using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.TokenWallets.TokenTransactions.Queries.GetUserDailyTransactions;
using RAGNET.Application.TokenWallets.Queries.GetUserTokenWallet;
using RAGNET.Application.TokenWallets.TokenTransactions.Queries.GetUserTransactions;
using RAGNET.Infrastructure.Processing;

namespace web.Controllers.Users
{
    [Route("api/wallet")]
    [ApiController]
    public class WalletController(
        QueriesExecutor queriesExecutor
    ) : ControllerBase
    {
        private readonly QueriesExecutor _queriesExecutor = queriesExecutor;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserWallet()
        {
            try
            {
                var walletQuery = new GetUserTokenWalletQuery();

                var wallet = await _queriesExecutor.Execute(walletQuery);

                return Ok(new
                {
                    wallet
                });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpGet("transactions")]
        [Authorize]
        public async Task<IActionResult> GetUserTransactions([FromQuery] GetUserTransactionsRequest request)
        {
            try
            {
                var transactionsQuery = new GetUserTransactionsQuery(request);

                var pagedResult = await _queriesExecutor.Execute(transactionsQuery);

                return Ok(new
                {
                    Transactions = pagedResult
                });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpGet("transactions/daily")]
        [Authorize]
        public async Task<IActionResult> GetUserDailyUsage([FromQuery] GetUserDailyTransactionsRequest request)
        {
            try
            {
                var dailyTransactionsQuery = new GetUserDailyTransactionsQuery(
                    request
                );

                var dailyTransactions = await _queriesExecutor.Execute(dailyTransactionsQuery);

                return Ok(new
                {
                    dailyTransactions
                });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}