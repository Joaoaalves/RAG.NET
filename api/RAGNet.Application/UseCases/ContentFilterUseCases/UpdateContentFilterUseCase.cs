using RAGNET.Application.DTOs.ContentFilter;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Filters;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.UseCases.ContentFilterUseCases
{
    public interface IUpdateContentFilterUseCase
    {
        Task<FilterDTO> Execute(Guid filterId, Filter data, string userId);
    }

    public class UpdateContentFilterUseCase(IFilterRepository _repo, IUnitOfWork unitOfWork) : IUpdateContentFilterUseCase
    {
        private readonly IFilterRepository _repo = _repo;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<FilterDTO> Execute(Guid filterId, Filter data, string userId)
        {
            try
            {
                var filter = await _repo.GetByIdAsync(filterId, userId) ?? throw new Exception("Filter not found.");

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