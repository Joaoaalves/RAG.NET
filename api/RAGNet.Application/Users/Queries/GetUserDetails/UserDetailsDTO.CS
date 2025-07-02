using RAGNET.Application.Subscriptions.DTOs;
using RAGNET.Application.TokenWallets.DTOs;

namespace RAGNET.Application.Users.Queries.GetUserDetails
{
    public class UserDetailsDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required TokenWalletDTO TokenWallet { get; set; }
        public required SubscriptionDTO Subscription { get; set; }
    }
}