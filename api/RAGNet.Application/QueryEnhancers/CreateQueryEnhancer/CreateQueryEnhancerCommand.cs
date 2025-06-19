using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryEnhancers;

namespace RAGNET.Application.QueryEnhancers.CreateQueryEnhancer
{
    public class CreateQueryEnhancerCommand(
        QueryEnhancer queryEnhancerDTO
    ) : ICommand<QueryEnhancerDTO>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public QueryEnhancer QueryEnhancer { get; } = queryEnhancerDTO;
    }
}