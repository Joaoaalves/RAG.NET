using RAGNET.Domain.Filters;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.UseCases.ContentFilterUseCases
{
    public interface ICreateContentFilterUseCase
    {
        Task<Filter> Execute(Filter filter, WorkflowId workflowId, string userId);
    }

    public class CreateContentFilterUseCase(IFilterRepository filterRepository, IUnitOfWork unitOfWork) : ICreateContentFilterUseCase
    {
        private readonly IFilterRepository _filterRepository = filterRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Filter> Execute(Filter filter, WorkflowId workflowId, string userId)
        {
            try
            {
                await _filterRepository.AddAsync(filter);
                await _unitOfWork.CommitAsync();
                return filter;
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