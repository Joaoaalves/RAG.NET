using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Repositories;

using RAGNET.Application.UseCases.EmbeddingUseCases;
using RAGNET.Application.UseCases.WorkflowUseCases;
using RAGNET.Application.Interfaces;
using RAGNET.Application.Services;

using RAGNET.Infrastructure.Data;
using RAGNET.Infrastructure.Adapters.Document;
using RAGNET.Infrastructure.Repositories;
using RAGNET.Infrastructure.Factories;
using RAGNET.Infrastructure.Adapters.VectorDB;
using RAGNET.Infrastructure.Adapters.OpenAIFactory;

using web.Configurations;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwaggerConfiguration();
builder.Services.AddControllers();

// Configure connection string in appsettings.json, for example:
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var signingKey = builder.Configuration["JWT:SignInKey"] ?? throw new Exception("You must set JWT SignInKey.");


// Register the ApplicationDbContext with PostgreSQL provider
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Repositories
builder.Services.AddScoped<IWorkflowRepository, WorkflowRepository>();
builder.Services.AddScoped<IChunkerRepository, ChunkerRepository>();
builder.Services.AddScoped<IQueryEnhancerRepository, QueryEnhancerRepository>();
builder.Services.AddScoped<IFilterRepository, FilterRepository>();
builder.Services.AddScoped<IRankerRepository, RankerRepository>();
builder.Services.AddScoped<IChunkRepository, ChunkRepository>();
builder.Services.AddScoped<IEmbeddingProviderConfigRepository, EmbeddingProviderConfigRepository>();

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

// Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPdfTextExtractorService, PdfTextExtractorAdapter>();

// Use Cases
builder.Services.AddScoped<IGetUserWorkflowsUseCase, GetUserWorkflowsUseCase>();
builder.Services.AddScoped<IProcessEmbeddingUseCase, ProcessEmbeddingUseCase>();
builder.Services.AddScoped<ICreateWorkflowUseCase, CreateWorkflowUseCase>();
builder.Services.AddScoped<IGetWorkflowUseCase, GetWorkflowUseCase>();

// Factories
builder.Services.AddScoped<ITextChunkerFactory, TextChunkerFactory>();
builder.Services.AddScoped<IEmbedderFactory, EmbedderFactory>();

// Adapters
builder.Services.AddScoped<IVectorDatabaseService, QDrantAdapter>();

// Controllers
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
public partial class Program { }