using AuraChat.Entities;
using AuraChat.Policies;
using AuraChat.Repositries.UserRepo;
using AuraChat.Services;
using AuraChat.Services.AuthServices;
using AuraChat.Services.EmailServices;
using AuraChat.Services.TokenServices;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        };

    c.AddSecurityRequirement(securityRequirement);
});



#region busniess services
builder.Services.AddScoped<IUserRepo, UserRepo>();

// auth services
builder.Services.AddScoped<ITokensService, TokensService>();
builder.Services.AddScoped<IAuthService, AuthService>();

#endregion


#region independent services

builder.Services.AddMemoryCache();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"));
    });

builder.Services.AddScoped<IEmailService, EmailService>();

#endregion


#region authorization releated DI
builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        var PrivateKey = Environment.GetEnvironmentVariable("PrivateKey")!;

        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(PrivateKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// policy registeration
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(RegisteredPolicies.GroupMemberPolicy, policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new GroupMemberRequirment());
    })
    .AddPolicy(RegisteredPolicies.GroupAdminPolicy, policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new GroupAdminRequirment());
    })
    .AddPolicy(RegisteredPolicies.FriendsPolicy, policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new FriendsRequirment());
    });
#endregion

var app = builder.Build();

var supportedCultures = new[] { "en", "ar" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    RequestCultureProviders =
    [
        new AcceptLanguageHeaderRequestCultureProvider(),
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider(),
    ]
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        DbInitializer.Seed(services.GetRequiredService<AppDbContext>());
    }
   
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseRequestLocalization(localizationOptions);

app.Run();

// thinking about adding 2FA on login since we will use email services anyway 
// basic setup in mind is to have an option for user to enable 2FA in settings (bool on user settings table)
// there will a middlware after the authorization middlware to intercept the request and check if the user has 2FA enabled
// if enabled, we will send an OTP (if non existant) to the user through the email he entered during registration and exit the pipeline
// the OTP will be stored in the cache system for 15 mins and will be checked against the user input
// if the user enters the correct OTP, we will allow the request to pass through the pipeline and provide the user with a JWT token

// NOTE: consider link redirection for the OTP email, so that the user can click on the link and the OTP will be automatically filled