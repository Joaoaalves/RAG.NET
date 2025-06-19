using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.QueryEnhancers.UpdateQueryEnhancer
{
    public class UpdateQueryEnhancerCommandHandler<TData>(
        IQueryEnhancerRepository queryEnhancerRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateQueryEnhancerCommand<TData>, QueryEnhancerDTO>
    where TData : UpdateQueryEnhancerRequest
    {
        private readonly IQueryEnhancerRepository _queryEnhancerRepository = queryEnhancerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<QueryEnhancerDTO> Handle(UpdateQueryEnhancerCommand<TData> request, CancellationToken cancellationToken)
        {
            try
            {
                var data = request.Data;
                var qe = await _queryEnhancerRepository.GetByIdAsync(request.QueryEnhancerId, request.UserId) ?? throw new Exception("Invalid Query Enhancer.");

                if (data is UpdateAutoQueryRequest auto)
                {
                    qe.UpdatePrompt(auto.Guidance);
                }

                qe.UpdateMaxQueries(data.MaxQueries);

                if (data.IsEnabled != null)
                    qe.SetEnableState(data.IsEnabled.Value);

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