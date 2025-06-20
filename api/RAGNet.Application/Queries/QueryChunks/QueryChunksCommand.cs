using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.DTOs.Query;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Queries.QueryChunks
{
    public class QueryChunksCommand(
        Workflow workflow,
        List<string> queries,
        QueryDTO queryDTO
    ) : ICommand<List<ContentItem>>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Workflow Workflow { get; } = workflow;
        public List<string> Queries { get; } = queries;
        public QueryDTO QueryDTO { get; } = queryDTO;
    }
}