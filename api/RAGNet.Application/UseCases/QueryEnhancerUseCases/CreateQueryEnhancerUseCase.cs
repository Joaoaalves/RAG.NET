using RAGNET.Application.DTOs.QueryEnhancer;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.QueryEnhancerUseCases
{
    public interface ICreateQueryEnhancerUseCase
    {
        Task<QueryEnhancer> Execute(QueryEnhancerCreationDTO dto, Guid workflowId);
    }

    public class CreateQueryEnhancerUseCase(IQueryEnhancerRepository queryEnhancerRepository) : ICreateQueryEnhancerUseCase
    {
        private readonly IQueryEnhancerRepository _queryEnhancerRepository = queryEnhancerRepository;
        public async Task<QueryEnhancer> Execute(QueryEnhancerCreationDTO dto, Guid workflowId)
        {
            var queryEnhancer = dto.ToQueryEnhancerFromCreationDTO(workflowId);
            await _queryEnhancerRepository.AddAsync(queryEnhancer);

            return queryEnhancer;
        }
    }
}