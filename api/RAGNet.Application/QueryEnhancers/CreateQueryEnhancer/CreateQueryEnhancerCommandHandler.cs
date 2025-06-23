using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.QueryEnhancers.CreateQueryEnhancer
{
    public class CreateQueryEnhancerCommandHandler(
        IQueryEnhancerRepository queryEnhancerRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateQueryEnhancerCommand, QueryEnhancerDTO>
    {
        private readonly IQueryEnhancerRepository _queryEnhancerRepository = queryEnhancerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<QueryEnhancerDTO> Handle(CreateQueryEnhancerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = request.User;
                var workflow = request.Workflow;

                if (workflow.QueryEnhancers.Any(qe => qe.Type == request.Type))
                    throw new Exception("Query Enhancer already enabled!");

                var queryEnhancer = QueryEnhancer.Create(
                    type: request.Type,
                    workflowId: workflow.Id,
                    userId: user.Id,
                    prompt: request.Prompt,
                    maxQueries: request.MaxQueries
                );

                await _queryEnhancerRepository.AddAsync(queryEnhancer);
                await _unitOfWork.CommitAsync(cancellationToken);

                return queryEnhancer.ToDTO();
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