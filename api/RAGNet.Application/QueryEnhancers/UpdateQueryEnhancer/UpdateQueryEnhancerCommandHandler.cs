using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.QueryEnhancers.UpdateQueryEnhancer
{
    public class UpdateQueryEnhancerCommandHandler(
        IQueryEnhancerRepository queryEnhancerRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateQueryEnhancerCommand, QueryEnhancerDTO>
    {
        private readonly IQueryEnhancerRepository _queryEnhancerRepository = queryEnhancerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<QueryEnhancerDTO> Handle(UpdateQueryEnhancerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var workflow = request.Workflow;

                var qe = workflow.QueryEnhancers.FirstOrDefault(qe => qe.Type == request.Strategy) ?? throw new Exception("Query Enhancer not enabled!");

                var queryEnhancer = await _queryEnhancerRepository.GetByIdAsync(qe.Id, request.User.Id) ?? throw new Exception("Invalid Query Enhancer.");

                if (request.Strategy == QueryEnhancerStrategy.AUTO_QUERY && request.Guidance != null)
                {
                    queryEnhancer.UpdatePrompt(request.Guidance);
                }

                queryEnhancer.UpdateMaxQueries(request.MaxQueries);

                if (request.IsEnabled != null)
                    queryEnhancer.SetEnableState(request.IsEnabled.Value);

                await _queryEnhancerRepository.UpdateAsync(queryEnhancer);

                await _unitOfWork.CommitAsync(cancellationToken);

                return queryEnhancer.ToDTO();
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