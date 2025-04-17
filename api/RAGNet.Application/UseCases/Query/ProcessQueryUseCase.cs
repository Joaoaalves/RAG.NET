using RAGNET.Application.DTOs.Query;
using RAGNET.Application.Mappers;
using RAGNET.Application.UseCases.QueryEnhancerUseCases;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Services.ApiKey;

namespace RAGNET.Application.UseCases.Query
{
    public interface IProcessQueryUseCase
    {
        Task<(List<ContentItem>, List<string>)> Execute(Workflow workflow, QueryDTO queryDTO);
    }

    public class ProcessQueryUseCase(
        IEnhanceQueryUseCase enhanceQueryUseCase,
        IQueryChunksUseCase queryChunksUseCase,
        IFilterContentUseCase filterContentUseCase,
        IApiKeyResolverService apiKeyResolverService
    ) : IProcessQueryUseCase
    {
        public async Task<(List<ContentItem>, List<string>)> Execute(Workflow workflow, QueryDTO queryDTO)
        {
            // Validate workflow
            if (workflow.EmbeddingProviderConfig == null)
                throw new Exception("You must set your embedding provider before querying.");

            if (workflow.ConversationProviderConfig == null)
                throw new Exception("You must set your conversation provider before querying.");

            // Resolve API keys for embedding and conversation providers
            var userEmbeddingProviderApiKey = await apiKeyResolverService.ResolveForUserAsync(
                workflow.UserId,
                workflow.EmbeddingProviderConfig.Provider.ToSupportedProvider()
            );

            var userConversationProviderApiKey = await apiKeyResolverService.ResolveForUserAsync(
                workflow.UserId,
                workflow.ConversationProviderConfig.Provider.ToSupportedProvider()
            );

            var queries = await enhanceQueryUseCase.Execute(workflow, queryDTO, userConversationProviderApiKey);

            // Fallback if no queries was generated
            if (queries.Count == 0)
            {
                queries.Add(queryDTO.Query);
            }

            var contentRetrieved = await queryChunksUseCase.Execute(
                workflow,
                queries,
                queryDTO,
                userEmbeddingProviderApiKey
            );

            var filteredText = await filterContentUseCase.Execute(contentRetrieved, workflow, queryDTO.Query, userConversationProviderApiKey);

            return (contentRetrieved, filteredText);
        }
    }
}