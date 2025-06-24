using Microsoft.AspNetCore.Mvc;

using RAGNET.Application.QueryEnhancers.Commands.EnhanceQuery;
using RAGNET.Application.Queries.DTOs;
using RAGNET.Application.Queries.Commands.QueryChunks;
using RAGNET.Application.Queries.Commands.FilterQueryResult;

using RAGNET.Infrastructure.Processing;

using web.Filters;
using RAGNET.Application.TokenWallets.Queries.GetUserTokenWallet;

namespace web.Controllers.Workflows.Queries
{
    [Route("/api/")]
    [ApiController]
    public class QueryController(
        CommandsExecutor commandsExecutor,
        QueriesExecutor queriesExecutor) : ControllerBase
    {
        private readonly CommandsExecutor _commandsExecutor = commandsExecutor;
        private readonly QueriesExecutor _queriesExecutor = queriesExecutor;

        [HttpPost("query")]
        [ServiceFilter(typeof(ApiWorkflowFilter))]
        public async Task<IActionResult> Query([FromBody] QueryDTO queryDTO)
        {
            try
            {
                var enhanceQuery = new EnhanceQueryCommand(queryDTO);
                var queries = await _commandsExecutor.Execute(enhanceQuery);

                var chunkQuery = new QueryChunksCommand(queries, queryDTO);
                var chunks = await _commandsExecutor.Execute(chunkQuery);

                var filterQuery = new FilterQueryResultCommand(chunks, queryDTO.Query);
                var filteredContent = await _commandsExecutor.Execute(filterQuery);

                var wallet = await _queriesExecutor.Execute(new GetUserTokenWalletQuery());

                return Ok(new { Chunks = chunks, FilteredContent = filteredContent, Wallet = wallet });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}