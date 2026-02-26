using DriveSync.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

using Microsoft.AspNetCore.Authentication.JwtBearer; // ✅ ADDED JWT
using Microsoft.IdentityModel.Tokens; // ✅ token validation
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(
builder.Configuration.GetConnectionString("DefaultConnection")));


// ✅ JWT Authentication Setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.SaveToken = true;

    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true, // ✅ USING JSON SETTINGS
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"], // ✅ FROM JSON

            ValidAudience =
                builder.Configuration["Jwt:Audience"], // ✅ FROM JSON

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]!)) // ✅ SECRET KEY
        };

    // ✅ VERY IMPORTANT — JWT READ FROM COOKIE
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token =
                context.Request.Cookies["jwt"];

            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    };
});


// ✅ Authorize Attribute Works
builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


// ✅ MUST BE BEFORE Authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();