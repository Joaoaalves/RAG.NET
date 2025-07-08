using RAGNET.Domain.SharedKernel.VectorStorages;

namespace RAGNET.Application.VectorStorages.DTOs
{
    public class VectorStorageDTO
    {
        public Guid Id { get; set; }
        public VectorStorageProvider Provider { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}