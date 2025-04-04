using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;

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
using RAGNET.Infrastructure.Services;
using UglyToad.PdfPig.Fonts.Encodings;
using RAGNET.Application.UseCases.QueryEnhancerUseCases;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter proper JWT token",
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "BearerAuth"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var isProductionEnv = Environment.GetEnvironmentVariable("PRODUCTION") ?? "false";
var isDevelopment = isProductionEnv == "false";

// Configure connection string in appsettings.json, for example:
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var signingKey = builder.Configuration["JWT:SignInKey"] ?? throw new Exception("You must set JWT SignInKey.");
var clientURL = Environment.GetEnvironmentVariable("CLIENT_URL") ?? "http://localhost:4200";

builder.Configuration.AddJsonFile("Configurations/prompts.json", optional: false, reloadOnChange: true);


// Register the ApplicationDbContext with PostgreSQL provider
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureAll<BearerTokenOptions>(option =>
{
    option.BearerTokenExpiration = TimeSpan.FromMinutes(60);
});

builder.Services.AddAuthorization();

// Repositories
builder.Services.AddScoped<IWorkflowRepository, WorkflowRepository>();
builder.Services.AddScoped<IChunkerRepository, ChunkerRepository>();
builder.Services.AddScoped<IQueryEnhancerRepository, QueryEnhancerRepository>();
builder.Services.AddScoped<IFilterRepository, FilterRepository>();
builder.Services.AddScoped<IRankerRepository, RankerRepository>();
builder.Services.AddScoped<IChunkRepository, ChunkRepository>();
builder.Services.AddScoped<IEmbeddingProviderConfigRepository, EmbeddingProviderConfigRepository>();
builder.Services.AddScoped<IConversationProviderConfigRepository, ConversationProviderConfigRepository>();

// Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPdfTextExtractorService, PdfTextExtractorAdapter>();
builder.Services.AddScoped<IEmbeddingProviderValidator, EmbeddingProviderValidator>();
builder.Services.AddScoped<IConversationProviderValidator, ConversationProviderValidator>();
builder.Services.AddScoped<IPromptService, PromptService>();

// Use Cases
builder.Services.AddScoped<IGetUserWorkflowsUseCase, GetUserWorkflowsUseCase>();
builder.Services.AddScoped<IProcessEmbeddingUseCase, ProcessEmbeddingUseCase>();
builder.Services.AddScoped<ICreateWorkflowUseCase, CreateWorkflowUseCase>();
builder.Services.AddScoped<IGetWorkflowUseCase, GetWorkflowUseCase>();
builder.Services.AddScoped<IDeleteWorkflowUseCase, DeleteWorkflowUseCase>();
builder.Services.AddScoped<ICreateQueryEnhancerUseCase, CreateQueryEnhancerUseCase>();

// Factories
builder.Services.AddScoped<ITextChunkerFactory, TextChunkerFactory>();
builder.Services.AddScoped<IChatCompletionFactory, ChatCompletionFactory>();
builder.Services.AddScoped<IEmbedderFactory, EmbedderFactory>();
builder.Services.AddScoped<IQueryEnhancerFactory, QueryEnhancerFactory>();

// Adapters
builder.Services.AddScoped<IVectorDatabaseService, QDrantAdapter>();

// Controllers
builder.Services.AddControllers();

// Cors
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policyBuilder =>
    {
        policyBuilder.AllowAnyHeader().AllowAnyMethod().WithOrigins(clientURL);
    });
});


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