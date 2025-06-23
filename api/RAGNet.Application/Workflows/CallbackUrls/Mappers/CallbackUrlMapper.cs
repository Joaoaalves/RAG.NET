using RAGNET.Application.Workflows.CallbackUrls.DTOs;
using RAGNET.Domain.SharedKernel.URLs;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Application.Workflows.CallbackUrls.Mappers
{
    public static class CallbackUrlMapper
    {

        public static CallbackUrlDTO ToDTO(this CallbackUrl callbackUrl)
        {
            return new CallbackUrlDTO
            {
                Id = callbackUrl.Id.Value,
                Url = callbackUrl.Url
            };
        }

        public static List<CallbackUrlDTO> ToDTOList(this IReadOnlyCollection<CallbackUrl> callbackUrlList)
        {
            List<CallbackUrlDTO> dtoList = [];

            foreach (var callbackUrl in callbackUrlList)
            {
                dtoList.Add(callbackUrl.ToDTO());
            }

            return dtoList;
        }

        public static List<string> ToUrlList(this List<URL> urls)
        {
            List<string> urlList = [];

            foreach (var url in urls)
            {
                urlList.Add(url.Value);
            }

            return urlList;
        }
    }
}