using AuraChat.Entities;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
Console.WriteLine(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"));
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// thinking about adding 2FA on login since we will use email services anyway 
// basic setup in mind is to have an option for user to enable 2FA in settings (bool on user settings table)
// there will a middlware after the authorization middlware to intercept the request and check if the user has 2FA enabled
// if enabled, we will send an OTP (if non existant) to the user through the email he entered during registration and exit the pipeline
// the OTP will be stored in the cache system for 15 mins and will be checked against the user input
// if the user enters the correct OTP, we will allow the request to pass through the pipeline and provide the user with a JWT token

// NOTE: consider link redirection for the OTP email, so that the user can click on the link and the OTP will be automatically filled