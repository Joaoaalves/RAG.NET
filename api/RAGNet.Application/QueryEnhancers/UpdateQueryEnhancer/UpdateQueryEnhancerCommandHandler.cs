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
                var qe = await _queryEnhancerRepository.GetByIdAsync(request.QueryEnhancerId, request.UserId) ?? throw new Exception("Invalid Query Enhancer.");

                if (request.Strategy == QueryEnhancerStrategy.AUTO_QUERY && request.Guidance != null)
                {
                    qe.UpdatePrompt(request.Guidance);
                }

                qe.UpdateMaxQueries(request.MaxQueries);

                if (request.IsEnabled != null)
                    qe.SetEnableState(request.IsEnabled.Value);

                await _queryEnhancerRepository.UpdateAsync(qe);

                await _unitOfWork.CommitAsync(cancellationToken);

                return qe.ToDTO();
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