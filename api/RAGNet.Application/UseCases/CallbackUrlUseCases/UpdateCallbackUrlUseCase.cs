using RAGNET.Application.DTOs.CallbackUrl;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.CallbackUrlUseCases
{
    public interface IUpdateCallbackUrlUseCase
    {
        Task<CallbackUrlDTO> Execute(CallbackUrlDTO dto, Guid callbackUrlId, Guid workflowId, string userId);
    }
    public class UpdateCallbackUrlUseCase(
        ICallbackUrlRepository callbackUrlRepository
    ) : IUpdateCallbackUrlUseCase
    {
        private readonly ICallbackUrlRepository _callbackUrlRepository = callbackUrlRepository;

        public async Task<CallbackUrlDTO> Execute(CallbackUrlDTO dto, Guid callbackUrlId, Guid workflowId, string userId)
        {
            var callback = await _callbackUrlRepository.GetByIdAsync(callbackUrlId, userId) ?? throw new Exception("Callback URL not found!");

            callback.Url = dto.Url;

            await _callbackUrlRepository.UpdateAsync(callback, userId);

            return callback.ToDTO();
        }

    }
}