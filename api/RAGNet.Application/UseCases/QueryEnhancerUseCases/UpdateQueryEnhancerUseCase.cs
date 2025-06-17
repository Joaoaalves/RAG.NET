using RAGNET.Application.DTOs.QueryEnhancer;
using RAGNET.Application.Mappers;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.UseCases.QueryEnhancerUseCases
{
    public interface IUpdateQueryEnhancerUseCase
    {
        Task<QueryEnhancerDTO> Execute(Guid queryEnhancerId, QueryEnhancer data, string userId);
    }

    public class UpdateQueryEnhancerUseCase(
        IQueryEnhancerRepository queryEnhancerRepository,
        IUnitOfWork unitOfWork) : IUpdateQueryEnhancerUseCase
    {
        private readonly IQueryEnhancerRepository _queryEnhancerRepository = queryEnhancerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<QueryEnhancerDTO> Execute(Guid queryEnhancerId, QueryEnhancer data, string userId)
        {
            try
            {
                var qe = await _queryEnhancerRepository.GetByIdAsync(queryEnhancerId, userId) ?? throw new Exception("Query enhancer not found.");

                qe.UpdateMaxQueries(data.MaxQueries);

                qe.UpdateMetas([.. data.Metas]);

                qe.SetEnableState(data.IsEnabled);

                await _queryEnhancerRepository.UpdateAsync(qe);

                await _unitOfWork.CommitAsync();

                return qe.ToQueryEnhancerDTO();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.RevertAsync();
                throw new Exception("Error updating query enhancer", ex);
            }
        }
    }
}