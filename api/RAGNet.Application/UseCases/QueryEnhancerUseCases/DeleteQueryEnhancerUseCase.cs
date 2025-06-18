using RAGNET.Application.DTOs.QueryEnhancer;
using RAGNET.Application.Mappers;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.UseCases.QueryEnhancerUseCases
{
    public interface IDeleteQueryEnhancerUseCase
    {
        Task<QueryEnhancerDTO> Execute(QueryEnhancerId queryEnhancerId, string userId);
    }

    public class DeleteQueryEnhancerUseCase(
        IQueryEnhancerRepository queryEnhancerRepository,
        IUnitOfWork unitOfWork) : IDeleteQueryEnhancerUseCase
    {
        private readonly IQueryEnhancerRepository _queryEnhancerRepository = queryEnhancerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<QueryEnhancerDTO> Execute(QueryEnhancerId queryEnhancerId, string userId)
        {
            try
            {
                var queryEnhancer = _queryEnhancerRepository.GetByIdAsync(queryEnhancerId, userId).Result ?? throw new Exception("Query enhancer not found.");

                await _queryEnhancerRepository.DeleteAsync(queryEnhancer);
                await _unitOfWork.CommitAsync();

                return queryEnhancer.ToQueryEnhancerDTO();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                await _unitOfWork.RevertAsync();
                throw new Exception("Error deleting query enhancer", exc);
            }
        }
    }
}