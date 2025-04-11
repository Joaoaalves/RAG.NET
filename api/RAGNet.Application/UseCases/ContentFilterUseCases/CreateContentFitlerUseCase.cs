using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.ContentFilterUseCases
{
    public interface ICreateContentFilterUseCase
    {
        Task<Filter> Execute(Filter filter, Guid workflowId, string userId);
    }

    public class CreateContentFilterUseCase(IFilterRepository filterRepository) : ICreateContentFilterUseCase
    {
        private readonly IFilterRepository _filterRepository = filterRepository;
        public async Task<Filter> Execute(Filter filter, Guid workflowId, string userId)
        {
            try
            {
                await _filterRepository.AddAsync(filter);
                return filter;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error creating query enhancer", ex);
            }
        }
    }
}