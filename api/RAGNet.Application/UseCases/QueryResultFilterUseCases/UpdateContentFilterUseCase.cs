using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.SeedWork;

using RAGNET.Application.DTOs.QueryResultFilter;
using RAGNET.Application.Mappers;
using RAGNET.Application.QueryResultFilters;

namespace RAGNET.Application.UseCases.QueryResultFilterUseCases
{
    public interface IUpdateQueryResultFilterUseCase
    {
        Task<QueryResultFilterDTO> Execute(QueryResultFilterId filterId, QueryResultFilter data, string userId);
    }

    public class UpdateQueryResultFilterUseCase(IQueryResultFilterRepository _repo, IUnitOfWork unitOfWork) : IUpdateQueryResultFilterUseCase
    {
        private readonly IQueryResultFilterRepository _repo = _repo;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<QueryResultFilterDTO> Execute(QueryResultFilterId filterId, QueryResultFilter data, string userId)
        {
            try
            {
                var filter = await _repo.GetByIdAsync(
                    filterId,
                    userId
                ) ?? throw new Exception("QueryResultFilter not found.");

                filter.UpdateMaxItems(data.MaxItems);
                filter.UpdateMetas([.. data.Metas]);
                filter.SetEnableState(data.IsEnabled);

                await _repo.UpdateAsync(filter, userId);
                await _unitOfWork.CommitAsync();
                return filter.ToDTO();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.RevertAsync();
                throw new Exception("Error updating content filter.", ex);
            }
        }
    }
}