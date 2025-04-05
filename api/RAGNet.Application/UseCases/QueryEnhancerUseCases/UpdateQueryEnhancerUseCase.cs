using RAGNET.Application.DTOs.QueryEnhancer;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.QueryEnhancerUseCases
{
    public interface IUpdateQueryEnhancerUseCase
    {
        Task<QueryEnhancerDTO> Execute(Guid queryEnhancerId, QueryEnhancer data, string userId);
    }

    public class UpdateQueryEnhancerUseCase(IQueryEnhancerRepository _repo) : IUpdateQueryEnhancerUseCase
    {
        private readonly IQueryEnhancerRepository _repo = _repo;
        public async Task<QueryEnhancerDTO> Execute(Guid queryEnhancerId, QueryEnhancer data, string userId)
        {
            try
            {
                var qe = await _repo.GetByIdAsync(queryEnhancerId, userId) ?? throw new Exception("Query enhancer not found.");

                qe.MaxQueries = data.MaxQueries;
                qe.Metas = data.Metas;
                qe.IsEnabled = data.IsEnabled;

                await _repo.UpdateAsync(qe, userId);
                return qe.ToQueryEnhancerDTO();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error updating query enhancer", ex);
            }
        }
    }
}