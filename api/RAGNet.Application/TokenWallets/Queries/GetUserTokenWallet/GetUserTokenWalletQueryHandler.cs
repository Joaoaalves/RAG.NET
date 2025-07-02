using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.TokenWallets.DTOs;
using RAGNET.Application.TokenWallets.Mappers;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.TokenWallets;

namespace RAGNET.Application.TokenWallets.Queries.GetUserTokenWallet
{
    public class GetUserTokenWalletQueryHandler(
        ITokenWalletRepository tokenWalletRepository
    ) : IQueryHandler<GetUserTokenWalletQuery, TokenWalletDTO>
    {
        private readonly ITokenWalletRepository _tokenWalletRepository = tokenWalletRepository;
        public async Task<TokenWalletDTO> Handle(GetUserTokenWalletQuery request, CancellationToken cancellationToken)
        {
            var userWallet = await _tokenWalletRepository.GetByUserIdAsync(request.User.Id) ?? throw new Exception("Wallet nof found!");

            return userWallet.ToDTO();
        }
    }
}