using RAGNET.Application.TokenWallets.TokenTransactions.DTOs;
using RAGNET.Domain.TokenWallets.TokenTransactions;

namespace RAGNET.Application.TokenWallets.TokenTransactions.Mappers
{
    public static class TokenTransationMapper
    {
        public static List<TokenTransactionDTO> ToDTOList(this List<TokenTransaction> transactions)
        {
            List<TokenTransactionDTO> dtoList = [];

            foreach (var transaction in transactions)
            {
                dtoList.Add(
                    new TokenTransactionDTO
                    {
                        Id = transaction.Id.Value,
                        WorkflowId = transaction.WorkflowId.Value,
                        OperationName = transaction.OperationName,
                        ContextInfo = transaction.ContextInfo,
                        Cost = transaction.Cost.Value,
                        TimeStamp = transaction.TimeStamp,
                        Source = transaction.Source
                    }
                );
            }

            return dtoList;
        }
    }
}