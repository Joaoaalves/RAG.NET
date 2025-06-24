using Microsoft.EntityFrameworkCore.Migrations.Operations;
using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.ProviderApiKeys.Services;
using RAGNET.Application.QueryResultFilters.Factories;
using RAGNET.Application.TokenWallets.Services;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.TokenWallets;

namespace RAGNET.Application.Queries.Commands.FilterQueryResult
{
    public class FilterQueryResultCommandHandler(
        IConversationProviderFactory chatCompletionFactory,
        IQueryResultFilterFactory queryResultFilterFactory,
        ITokenCostCalculator tokenCostCalculator,
        ITokenWalletRepository tokenWalletRepository,
        IApiKeyResolverService apiKeyResolverService,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<FilterQueryResultCommand, List<string>>
    {
        private readonly IConversationProviderFactory _chatCompletionFactory = chatCompletionFactory;
        private readonly IQueryResultFilterFactory _queryResultFilterFactory = queryResultFilterFactory;
        private readonly IApiKeyResolverService _apiKeyResolverService = apiKeyResolverService;
        private readonly ITokenCostCalculator _tokenCostCalculator = tokenCostCalculator;
        private readonly ITokenWalletRepository _tokenWalletRepository = tokenWalletRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<string>> Handle(
            FilterQueryResultCommand request,
            CancellationToken cancellationToken
        )
        {
            var workflow = request.Workflow;
            var wallet = request.TokenWallet;

            if (workflow.QueryResultFilter == null || !workflow.QueryResultFilter!.IsEnabled)
                return [];

            var userConversationProviderApiKey = await _apiKeyResolverService.ResolveForUserAsync(
                workflow.UserId,
                workflow.ConversationProviderConfig.Provider
            );

            var completionProvider = _chatCompletionFactory.CreateCompletionService(
                userConversationProviderApiKey,
                workflow.ConversationProviderConfig
            );

            var queryResultFilterService = _queryResultFilterFactory
            .CreateQueryResultFilter(workflow.QueryResultFilter);

            // Costs
            var estimatedCost = _tokenCostCalculator.Calculate(queryResultFilterService, request.Items);

            wallet.Consume(
                estimatedCost,
                "Query Filter",
                $"WorkflowId={workflow.Id.Value};QueryFilter={workflow.QueryResultFilter.Strategy}"
            );

            await _tokenWalletRepository.UpdateAsync(wallet);

            await _unitOfWork.CommitAsync(cancellationToken);

            return await queryResultFilterService.FilterContent(
                request.Items,
                request.Query,
                completionProvider
            );
        }
    }
}