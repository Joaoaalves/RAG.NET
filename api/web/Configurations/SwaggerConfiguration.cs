using Microsoft.OpenApi.Models;

namespace web.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "RAG.NET API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                });

                option.OperationFilter<FileUploadOperation>();

                // API KEY
                option.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "Add you API Key on field. E.g: 123456abcdef",
                    Type = SecuritySchemeType.ApiKey,
                    Name = "x-api-key",
                    In = ParameterLocation.Header,
                    Scheme = "ApiKeyScheme"
                });

                // Register the OperationFilter that adds the API key only for endpoints with [ApiKeyCheck]
                option.OperationFilter<ApiKeyOperationFilter>();
            });

        }
    }
}