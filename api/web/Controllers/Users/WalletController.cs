using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.TokenWallets.DTOs;
using RAGNET.Application.TokenWallets.Queries.GetUserDailyTransactions;
using RAGNET.Application.TokenWallets.Queries.GetUserTokenWallet;
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
        public async Task<IActionResult> GetUserWalletInfo([FromQuery] GetUserWalletInfoDTO dto)
        {
            try
            {
                DateTime date = DateTime.UtcNow;
                DateTime startDate = dto.Start ?? new DateTime(date.Year, date.Month, 1);
                DateTime endDate = dto.End ?? date;

                var walletQuery = new GetUserTokenWalletQuery();

                var wallet = await _queriesExecutor.Execute(walletQuery);

                var dailyTransactionsQuery = new GetUserDailyTransactionsQuery(
                    startDate,
                    endDate
                );

                var dailyTransactions = await _queriesExecutor.Execute(dailyTransactionsQuery);

                return Ok(new
                {
                    wallet,
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