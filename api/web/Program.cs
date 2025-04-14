using Microsoft.EntityFrameworkCore;

using RAGNET.Domain.Entities;

using web.Configurations;
using web.Extensions;

var builder = WebApplication.CreateBuilder(args);

var isProductionEnv = Environment.GetEnvironmentVariable("PRODUCTION") ?? "false";
var isDevelopment = isProductionEnv == "false";

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var signingKey = builder.Configuration["JWT:SignInKey"] ?? throw new Exception("You must set JWT SignInKey.");
var clientURL = Environment.GetEnvironmentVariable("CLIENT_URL") ?? "http://localhost:4200";

builder.Configuration.AddJsonFile("Configurations/prompts.json", optional: false, reloadOnChange: true);

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddAuthConfiguration(builder.Configuration);

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Cors
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policyBuilder =>
    {
        policyBuilder.AllowAnyHeader().AllowAnyMethod().WithOrigins(clientURL);
    });
});

builder.Services.AddSwaggerConfiguration();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRepositoryConfiguration();
builder.Services.AddServiceConfiguration();
builder.Services.AddAdapterConfiguration();
builder.Services.AddFilterConfiguration();
builder.Services.AddUseCaseConfiguration();
builder.Services.AddFactoryConfiguration();

var app = builder.Build();

if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

var identityApi = app.MapIdentityApi<User>();


app.MapControllers();

app.Run();
public partial class Program { }