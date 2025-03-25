using Microsoft.OpenApi.Models;
using RAGNET.Application.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace web.Configurations
{
    public class ApiKeyOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Checks if the current method has the attribute ApiKeyCheckAttribute
            var hasApiKeyAttribute = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<ApiKeyCheckAttribute>()
                .Any();

            // if dont, doesn't add the security schema
            if (!hasApiKeyAttribute)
                return;

            // Creates the security schema
            operation.Security =
        [
            new OpenApiSecurityRequirement
            {
                [ new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" },
                        In = ParameterLocation.Header,
                    }
                ] = []
            }
        ];
        }
    }
}