using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.QueryEnhancerUseCases
{
    public interface IDeleteQueryEnhancerUseCase
    {
        Task<bool> Execute(Guid queryEnhancerId, string userId);
    }

    public class DeleteQueryEnhancerUseCase(IQueryEnhancerRepository repo) : IDeleteQueryEnhancerUseCase
    {
        private readonly IQueryEnhancerRepository _repo = repo;
        public Task<bool> Execute(Guid queryEnhancerId, string userId)
        {
            try
            {
                var queryEnhancer = _repo.GetByIdAsync(queryEnhancerId, userId).Result ?? throw new Exception("Query enhancer not found.");

                _repo.DeleteAsync(queryEnhancer, userId).Wait();
                return Task.FromResult(true);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                throw new Exception("Error deleting query enhancer", exc);
            }
        }
    }
}