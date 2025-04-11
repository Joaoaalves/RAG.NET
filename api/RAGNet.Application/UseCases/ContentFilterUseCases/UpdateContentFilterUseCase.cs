using RAGNET.Application.DTOs.ContentFilter;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.ContentFilterUseCases
{
    public interface IUpdateContentFilterUseCase
    {
        Task<FilterDTO> Execute(Guid filterId, Filter data, string userId);
    }

    public class UpdateContentFilterUseCase(IFilterRepository _repo) : IUpdateContentFilterUseCase
    {
        private readonly IFilterRepository _repo = _repo;
        public async Task<FilterDTO> Execute(Guid filterId, Filter data, string userId)
        {
            try
            {
                var qe = await _repo.GetByIdAsync(filterId, userId) ?? throw new Exception("Filter not found.");

                qe.MaxItems = data.MaxItems;
                qe.Metas = data.Metas;
                qe.IsEnabled = data.IsEnabled;

                await _repo.UpdateAsync(qe, userId);
                return qe.ToDTO();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error updating content filter.", ex);
            }
        }
    }
}