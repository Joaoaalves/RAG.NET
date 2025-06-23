using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.QueryEnhancers.Commands.DeleteQueryEnhancer
{
    public class DeleteQueryEnhancerCommandHandler(
        IQueryEnhancerRepository queryEnhancerRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<DeleteQueryEnhancerCommand, bool>
    {
        private readonly IQueryEnhancerRepository _queryEnhancerRepository = queryEnhancerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Handle(DeleteQueryEnhancerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var workflow = request.Workflow;

                var qe = workflow.QueryEnhancers.FirstOrDefault(qe => qe.Type == request.Strategy) ?? throw new Exception("Query Enhancer is not enabled!");

                var queryEnhancer = _queryEnhancerRepository.GetByIdAsync(qe.Id, request.User.Id).Result ?? throw new Exception("Query enhancer not found.");

                await _queryEnhancerRepository.DeleteAsync(queryEnhancer);

                await _unitOfWork.CommitAsync(cancellationToken);

                return true;
            }
            catch (Exception exc)
            {
                await _unitOfWork.RevertAsync();
                throw new Exception("Error deleting query enhancer", exc);
            }
        }
    }
}