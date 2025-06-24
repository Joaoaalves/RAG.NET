using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Infrastructure.Providers.Conversation.Mappers;
using RAGNET.Application.ProviderApiKeys.Services;
using RAGNET.Application.QueryEnhancers.Factories;
using RAGNET.Application.TokenWallets.Services;

using RAGNET.Domain.SeedWork;
using RAGNET.Domain.TokenWallets;

namespace RAGNET.Application.QueryEnhancers.Commands.EnhanceQuery
{
    public class EnhanceQueryCommandHandler(
        IQueryEnhancerFactory queryEnhancerFactory,
        IConversationProviderFactory chatCompletionFactory,
        ITokenWalletRepository tokenWalletRepository,
        ITokenCostCalculator tokenCostCalculator,
        IUnitOfWork unitOfWork,
        IApiKeyResolverService apiKeyResolverService
    ) : ICommandHandler<EnhanceQueryCommand, List<string>>
    {
        private readonly IQueryEnhancerFactory _queryEnhancerFactory = queryEnhancerFactory;
        private readonly IConversationProviderFactory _chatCompletionFactory = chatCompletionFactory;
        private readonly IApiKeyResolverService _apiKeyResolverService = apiKeyResolverService;
        private readonly ITokenCostCalculator _tokenCostCalculator = tokenCostCalculator;
        private readonly ITokenWalletRepository _tokenWalletRepository = tokenWalletRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<string>> Handle(EnhanceQueryCommand request, CancellationToken cancellationToken)
        {
            var workflow = request.Workflow;
            try
            {
                if (workflow.QueryEnhancers == null || workflow.QueryEnhancers.Count == 0)
                    return [
                        request.QueryDTO.Query
                    ];

                var wallet = await _tokenWalletRepository.GetByUserIdAsync(workflow.UserId) ?? throw new Exception("Wallet not found!");

                var userConversationProviderApiKey = await _apiKeyResolverService.ResolveForUserAsync(
                    workflow.UserId,
                    workflow.ConversationProviderConfig.Provider.ToSupportedProvider()
                );

                var completionService = _chatCompletionFactory.CreateCompletionService(
                    userConversationProviderApiKey,
                    workflow.ConversationProviderConfig
                );

                var tasks = workflow.QueryEnhancers.Select(async qeConfig =>
                {
                    if (qeConfig.IsEnabled)
                    {
                        var queryEnhancer = _queryEnhancerFactory.CreateQueryEnhancer(qeConfig, completionService);

                        var estimatedCost = _tokenCostCalculator.Calculate(queryEnhancer, qeConfig.MaxQueries);

                        wallet.Consume(estimatedCost, "Query Enhancer", $"workflowId={workflow.Id.Value};QueryEnhancer={qeConfig.Type}");

                        return await queryEnhancer.GenerateQueries(request.QueryDTO.Query);
                    }

                    return null;

                }).ToList();

                var results = await Task.WhenAll(tasks);
                await _tokenWalletRepository.UpdateAsync(wallet);
                await _unitOfWork.CommitAsync(cancellationToken);

                return results
                    .Where(result => result != null)
                    .SelectMany(result => result ?? [])
                    .ToList();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                throw new Exception("An error occurred while processing your query");
            }
        }
    }
}