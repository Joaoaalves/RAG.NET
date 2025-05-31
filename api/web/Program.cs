using Microsoft.EntityFrameworkCore;

using RAGNET.Domain.Entities;
using RAGNET.Infrastructure.Adapters.SignalR;
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
builder.Services.AddAuthConfiguration();

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Processing Handlers
builder.Services.AddHandlerConfiguration();

// Cors
builder.Services.AddCorsConfiguration();

builder.Services.AddSwaggerConfiguration();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRepositoryConfiguration();
builder.Services.AddServiceConfiguration();
builder.Services.AddAdapterConfiguration(builder.Configuration);
builder.Services.AddFilterConfiguration();
builder.Services.AddUseCaseConfiguration();
builder.Services.AddFactoryConfiguration();

// Hub
builder.Services.AddSignalR();

var app = builder.Build();

if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    var origin = context.Request.Headers["Origin"].ToString();

    if (!string.IsNullOrEmpty(origin) &&
        (origin == "http://localhost:4200" || origin == "http://192.168.0.52:4200"))
    {
        context.Items["CorsPolicyName"] = "FrontendPolicy";
    }
    else
    {
        context.Items["CorsPolicyName"] = "PublicPolicy";
    }

    await next();
});

// Dynamic CORS
app.UseWhen(
    context =>
    {
        var origin = context.Request.Headers.Origin.ToString();

        if (string.IsNullOrWhiteSpace(origin))
            return false;

        try
        {
            var uri = new Uri(origin);
            return uri.Host.StartsWith("192.168.0.") || uri.Host == "localhost";
        }
        catch
        {
            return false;
        }
    },
    appBuilder =>
    {
        appBuilder.UseCors("CorsPolicy");
    });

app.UseCors("PublicPolicy");

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

// Jobs (Just embedding ATM)
app.MapHub<JobStatusHub>("/hubs/jobstatus");

var identityApi = app.MapIdentityApi<User>();


app.MapControllers();

app.Run();
public partial class Program { }