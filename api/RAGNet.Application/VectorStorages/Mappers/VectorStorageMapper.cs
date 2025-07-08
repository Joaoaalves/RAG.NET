using RAGNET.Application.VectorStorages.DTOs;
using RAGNET.Domain.VectorStorages;

namespace RAGNET.Application.VectorStorages.Mappers
{
    public static class VectorStorageMapper
    {
        public static VectorStorageDTO ToDTO(this VectorStorage vectorStorage)
        {
            return new VectorStorageDTO
            {
                Id = vectorStorage.Id.Value,
                Provider = vectorStorage.Provider
            };
        }
    }
}