using AuraChat.Entities;
using AuraChat.MiddleWares;
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
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

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

// global rate limiter per any type of user in the future might split to auth and non authed user
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.FindFirstValue("id") ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                // limit the window to 10 requests per minute
                AutoReplenishment = true,
                PermitLimit = 10,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
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
        x.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                // Resolve the UserManager (or your service)
                var userRepo = context.HttpContext.RequestServices
                    .GetRequiredService<IUserRepo>();

                // Get the user's ID and pwd_version from the token
                var userId = context.Principal!.FindFirstValue("id");
                var passwordChangeTracker = context.Principal!.FindFirstValue("PCT");

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(passwordChangeTracker))
                {
                    context.Fail("Invalid token");
                    return;
                }

                // Fetch the user from the database
                var user = await userRepo.GetByIdAsync(int.Parse(userId));
                if (user == null || user.UserSettings.PasswordChangeCounter != int.Parse(passwordChangeTracker))
                {
                    context.Fail("Token invalidated due to password change");
                }
            }
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

app.UseRequestLocalization(localizationOptions);

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseRouting();

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<AuditLoggerMiddleware>();

app.MapControllers();

app.Run();
