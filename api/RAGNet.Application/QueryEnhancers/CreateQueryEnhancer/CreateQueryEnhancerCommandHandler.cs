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
                var queryEnhancer = await _queryEnhancerRepository.AddAsync(request.QueryEnhancer);
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