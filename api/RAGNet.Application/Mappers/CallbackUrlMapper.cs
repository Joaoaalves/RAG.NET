using RAGNET.Application.DTOs.CallbackUrl;
using RAGNET.Domain.Entities;

namespace RAGNET.Application.Mappers
{
    public static class CallbackUrlMapper
    {
        public static CallbackUrl ToCallbackUrl(this CallbackUrlDTO dto, Guid workflowId, string userId)
        {
            return new CallbackUrl
            {
                Id = dto.Id ?? Guid.NewGuid(),
                Url = dto.Url,
                WorkflowId = workflowId,
                UserId = userId
            };
        }

        public static CallbackUrlDTO ToDTO(this CallbackUrl callbackUrl)
        {
            return new CallbackUrlDTO
            {
                Id = callbackUrl.Id,
                Url = callbackUrl.Url
            };
        }

        public static List<CallbackUrlDTO> ToDTOList(this ICollection<CallbackUrl> callbackUrlList)
        {
            List<CallbackUrlDTO> dtoList = [];

            foreach (var callbackUrl in callbackUrlList)
            {
                dtoList.Add(callbackUrl.ToDTO());
            }

            return dtoList;
        }
    }
}