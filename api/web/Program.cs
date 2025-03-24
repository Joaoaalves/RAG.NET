using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using RAGNET.Domain.Entities;
using RAGNET.Infrastructure.Data;
using RAGNET.Application.Interfaces;

using web.Configurations;
using RAGNET.Application.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwaggerConfiguration();
builder.Services.AddControllers();

// Configure connection string in appsettings.json, for example:
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var signingKey = builder.Configuration["JWT:SignInKey"] ?? throw new Exception("You must set JWT SignInKey.");


// Register the ApplicationDbContext with PostgreSQL provider
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configure Identity with the custom User entity
builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        // Configure Identity options as needed (e.g., password settings)
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(signingKey)
        )
    };
});



builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RAG.NET API V1");
});

app.UseCors(x => x
     .AllowAnyMethod()
     .AllowAnyHeader()
     .AllowCredentials()
      .SetIsOriginAllowed(origin => true));

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
