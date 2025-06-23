using System.Reflection;
using FluentValidation;

using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.SeedWork;

using RAGNET.Application.Users;
using RAGNET.Application.Queries;
using RAGNET.Application.Providers;
using RAGNET.Application.Chunkers;
using RAGNET.Application.ProviderApiKeys;
using RAGNET.Application.Configuration.Validation;
using RAGNET.Application.Workflows.CreateWorkflow;
using RAGNET.Application.Providers.Embedding;
using RAGNET.Application.Providers.Conversation;

using RAGNET.Infrastructure.Embedders;
using RAGNET.Infrastructure.ChatCompletions;
using RAGNET.Infrastructure.Providers;
using RAGNET.Infrastructure.Processing;
using RAGNET.Application.Configuration.Commands.Behaviors;
using RAGNET.Application.Configuration.Queries.Behaviors;

namespace web.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services, params object[] args)
        {
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IEmbeddingProviderResolver, EmbeddingProviderResolver>();
            services.AddScoped<IConversationProviderResolver, ConversationProviderResolver>();

            services.AddScoped<IPromptService, PromptService>();

            services.AddScoped<IQueryResultAggregatorService, QueryResultAggregatorService>();

            services.AddScoped<IEmbeddingProcessingService, EmbeddingProcessingService>();

            services.AddScoped<IChunkRetrieverService, ChunkRetrieverService>();

            services.AddScoped<IScoreNormalizerService, ScoreNormalizerService>();

            // ApiKey
            services.AddScoped<IApiKeyResolverService, ApiKeyResolverService>();
            services.AddScoped<ICryptoService, CryptoService>();

            services.AddSingleton<IProviderModelCatalogService, ProviderModelCatalogService>();

            // Mediator
            var assemblies = ResolveAssemblies(args);
            services.AddScoped<IMediator, Mediator>();
            RegisterHandlers(services, assemblies, typeof(INotificationHandler<>));
            RegisterHandlers(services, assemblies, typeof(IRequestHandler<,>));

            services.AddScoped<CommandsExecutor>();
            services.AddScoped<QueriesExecutor>();

            services.AddScoped(typeof(IRequestPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddValidatorsFromAssemblyContaining<CreateWorkflowCommandValidator>();

            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

            services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(WorkflowInjectionBehavior<,>));
            services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(UserInjectionCommandBehavior<,>));
            services.AddScoped(typeof(IRequestPipelineBehavior<,>), typeof(UserInjectionQueryBehavior<,>));

            services.AddHttpContextAccessor();

            return services;
        }

        public static Assembly[] ResolveAssemblies(object[] args)
        {
            if (args == null || args.Length == 0)
                return AppDomain.CurrentDomain
                        .GetAssemblies()
                        .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.FullName))
                        .ToArray();


            if (args.All(a => a is Assembly))
                return args.Cast<Assembly>().ToArray();

            if (args.All(a => a is string))
            {
                var prefixes = args.Cast<string>().ToArray();

                return AppDomain.CurrentDomain
                        .GetAssemblies()
                        .Where(a =>
                            !a.IsDynamic &&
                            !string.IsNullOrWhiteSpace(a.FullName) &&
                            prefixes.Any(p => a.FullName!.StartsWith(p))
                        ).ToArray();
            }

            throw new ArgumentException("Invalid Parameters for AddMediator().");
        }

        public static void RegisterHandlers(IServiceCollection services, Assembly[] assemblies, Type handlerInterface)
        {
            var types = assemblies.SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces()
                    .Where(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == handlerInterface
                    );

                foreach (var intrfc in interfaces)
                {
                    services.AddTransient(intrfc, type);
                }
            }
        }

    }
}