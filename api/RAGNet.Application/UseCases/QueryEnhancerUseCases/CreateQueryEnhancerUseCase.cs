using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.UseCases.QueryEnhancerUseCases
{
    public interface ICreateQueryEnhancerUseCase
    {
        Task<QueryEnhancer> Execute(QueryEnhancer queryEnhancer, WorkflowId workflowId, string userId);
    }

    public class CreateQueryEnhancerUseCase(
        IQueryEnhancerRepository queryEnhancerRepository,
        IUnitOfWork unitOfWork) : ICreateQueryEnhancerUseCase
    {
        private readonly IQueryEnhancerRepository _queryEnhancerRepository = queryEnhancerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<QueryEnhancer> Execute(QueryEnhancer queryEnhancer, WorkflowId workflowId, string userId)
        {
            try
            {
                await _queryEnhancerRepository.AddAsync(queryEnhancer);
                await _unitOfWork.CommitAsync();
                return queryEnhancer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.RevertAsync();
                throw new Exception("Error creating query enhancer", ex);
            }
        }
    }
}