using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Queries.QueryChunks
{
    public class QueryChunksCommand(
        List<string> queries,
        QueryDTO queryDTO
    ) : WorkflowAwareCommand<List<ContentItem>>
    {
        public List<string> Queries { get; } = queries;
        public QueryDTO QueryDTO { get; } = queryDTO;
    }
}