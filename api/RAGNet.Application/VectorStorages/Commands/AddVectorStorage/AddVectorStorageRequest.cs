using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Domain.Users.ApiKeys;

namespace RAGNET.Application.VectorStorages.Commands.AddVectorStorage
{
    public class AddVectorStorageRequest
    {
        public ApiKey ApiKey { get; set; } = null!;
        public VectorStorageProvider Provider { get; set; }
        public string? Host { get; set; }
        public string? PodSize { get; set; }
        public string? PodType { get; set; }
        public uint? Pods { get; set; }
        public string? Environment { get; set; }
        public string? Cloud { get; set; }
        public string? Region { get; set; }
    }
}