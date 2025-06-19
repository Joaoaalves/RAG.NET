using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.DTOs.Query;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.QueryEnhancers.EnhanceQuery
{
    public class EnhanceQueryCommand(
        Workflow workflow,
        QueryDTO queryDTO
    ) : ICommand<List<string>>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Workflow Workflow { get; } = workflow;
        public QueryDTO QueryDTO { get; } = queryDTO;
    }
}