using RAGNET.Application.DTOs.CallbackUrl;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.CallbackUrlUseCases
{
    public interface IAddCallbackUrlUseCase
    {
        Task<CallbackUrlDTO> Execute(CallbackUrlDTO dto, Guid workflowId, string userId);
    }
    public class AddCallbackUrlUseCase(
        ICallbackUrlRepository callbackUrlRepository
    ) : IAddCallbackUrlUseCase
    {
        private readonly ICallbackUrlRepository _callbackUrlRepository = callbackUrlRepository;

        public async Task<CallbackUrlDTO> Execute(CallbackUrlDTO dto, Guid workflowId, string userId)
        {
            var callback = await _callbackUrlRepository.AddAsync(dto.ToCallbackUrl(workflowId, userId)) ?? throw new Exception("Failed to add Callback URL to Workflow");

            return callback.ToDTO();
        }

    }
}