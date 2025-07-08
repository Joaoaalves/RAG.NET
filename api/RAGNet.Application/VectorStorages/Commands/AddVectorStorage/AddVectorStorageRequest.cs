using RAGNET.Domain.SharedKernel.VectorStorages;

namespace RAGNET.Application.VectorStorages.Commands.AddVectorStorage
{
    public class AddVectorStorageRequest
    {
        public string ApiKey { get; set; } = null!;
        public VectorStorageProvider Provider { get; set; }
        public int? IndexType { get; set; }
        public string? Host { get; set; }
        public string? PodSize { get; set; }
        public string? PodType { get; set; }
        public uint? Pods { get; set; }
        public string? Environment { get; set; }
        public int? Cloud { get; set; }
        public string? Region { get; set; }
    }
}