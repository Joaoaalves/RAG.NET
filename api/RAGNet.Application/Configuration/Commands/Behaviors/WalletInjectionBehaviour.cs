using Microsoft.AspNetCore.Http;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Configuration.Commands.Behaviors
{
    public class WalletInjectionBehavior<TCommand, TResult>(
        IHttpContextAccessor httpContextAccessor,
        ITokenWalletRepository tokenWalletRepository
    ) : ICommandPipelineBehavior<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        private readonly ITokenWalletRepository _tokenWalletRepository = tokenWalletRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public async Task<TResult> Handle(
            TCommand command,
            Func<TCommand, Task<TResult>> next,
            CancellationToken cancellationToken
        )
        {
            if (command is IWalletAware walletAware)
            {
                if (_httpContextAccessor.HttpContext?.Items["Workflow"] is not Workflow workflow)
                    throw new Exception("Workflow not found");

                var wallet = await _tokenWalletRepository.GetByUserIdAsync(workflow.UserId) ?? throw new Exception("Wallet not found");

                walletAware.InjectWallet(wallet);
            }

            return await next(command);
        }
    }
}