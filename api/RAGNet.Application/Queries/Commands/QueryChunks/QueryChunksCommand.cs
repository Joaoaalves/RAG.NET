using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Queries.DTOs;

namespace RAGNET.Application.Queries.Commands.QueryChunks
{
    public class QueryChunksCommand(
        List<string> queries,
        QueryDTO queryDTO
    ) : WorkflowAwareCommand<List<ContentItemDTO>>
    {
        public List<string> Queries { get; } = queries;
        public QueryDTO QueryDTO { get; } = queryDTO;
    }
}