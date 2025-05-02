using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.CallbackUrlUseCases
{
    public interface IDeleteCallbackUrlUseCase
    {
        Task<Guid> Execute(Guid callbackUrlId, string userId);
    }
    public class DeleteCallbackUrlUseCase(
        ICallbackUrlRepository callbackUrlRepository
    ) : IDeleteCallbackUrlUseCase
    {
        private readonly ICallbackUrlRepository _callbackUrlRepository = callbackUrlRepository;

        public async Task<Guid> Execute(Guid callbackUrlId, string userId)
        {
            var callback = await _callbackUrlRepository.GetByIdAsync(callbackUrlId, userId) ?? throw new Exception("Invalid Callback URl.");

            await _callbackUrlRepository.DeleteAsync(callback, userId);

            return callbackUrlId;
        }

    }
}