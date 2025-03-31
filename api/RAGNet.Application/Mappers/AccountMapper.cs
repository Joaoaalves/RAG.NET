using RAGNET.Application.DTOs.Account;
using RAGNET.Domain.Entities;

namespace RAGNET.Application.Mappers
{
    public static class AccountMapper
    {
        public static AccountInfoDTO ToAccountInfoDTOFromUser(this User user)
        {
            return new AccountInfoDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? ""
            };
        }
    }
}