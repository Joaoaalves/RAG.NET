using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.VectorStorages.DTOs;
using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Domain.Users.ApiKeys;

namespace RAGNET.Application.VectorStorages.Commands.AddVectorStorage
{
    public class AddVectorStorageCommand(
        AddVectorStorageRequest request
    ) : UserAwareCommand<VectorStorageDTO>
    {
        public ApiKey ApiKey { get; set; } = new ApiKey(request.ApiKey);
        public VectorStorageProvider Provider { get; set; } = request.Provider;
        public int? IndexType { get; set; } = request.IndexType;
        public string? Host { get; set; } = request.Host;
        public string? PodSize { get; set; } = request.PodSize;
        public string? PodType { get; set; } = request.PodType;
        public uint? Pods { get; set; } = request.Pods;
        public string? Environment { get; set; } = request.Environment;
        public int? Cloud { get; set; } = request.Cloud;
        public string? Region { get; set; } = request.Region;
    }
}