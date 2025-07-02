using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Queries.DTOs;

namespace RAGNET.Application.QueryEnhancers.Commands.EnhanceQuery
{
    public class EnhanceQueryCommand(
        QueryDTO queryDTO
    ) : WorkflowAndWalletAwareCommand<List<string>>
    {
        public QueryDTO QueryDTO { get; } = queryDTO;

    }
}