using RAGNET.Application.DTOs.QueryEnhancer;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.QueryEnhancerUseCases
{
    public interface ICreateQueryEnhancerUseCase
    {
        Task<QueryEnhancer> Execute(QueryEnhancer queryEnhancer, Guid workflowId, string userId);
    }

    public class CreateQueryEnhancerUseCase(IQueryEnhancerRepository queryEnhancerRepository) : ICreateQueryEnhancerUseCase
    {
        private readonly IQueryEnhancerRepository _queryEnhancerRepository = queryEnhancerRepository;
        public async Task<QueryEnhancer> Execute(QueryEnhancer queryEnhancer, Guid workflowId, string userId)
        {
            try
            {
                await _queryEnhancerRepository.AddAsync(queryEnhancer);
                return queryEnhancer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error creating query enhancer", ex);
            }
        }
    }
}