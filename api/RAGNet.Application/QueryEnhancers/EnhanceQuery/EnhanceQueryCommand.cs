using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Queries;

namespace RAGNET.Application.QueryEnhancers.EnhanceQuery
{
    public class EnhanceQueryCommand(
        QueryDTO queryDTO
    ) : WorkflowAwareCommand<List<string>>
    {
        public QueryDTO QueryDTO { get; } = queryDTO;

    }
}