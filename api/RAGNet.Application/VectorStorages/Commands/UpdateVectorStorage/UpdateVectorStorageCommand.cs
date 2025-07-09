using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.VectorStorages.DTOs;
using RAGNET.Domain.Users.ApiKeys;
using RAGNET.Domain.VectorStorages;

namespace RAGNET.Application.VectorStorages.Commands.UpdateVectorStorage
{
    public class UpdateVectorStorageCommand(
        UpdateVectorStorageRequest request,
        VectorStorageId vectorStorageId
    ) : UserAwareCommand<VectorStorageDTO>
    {

        public VectorStorageId VectorStorageId { get; set; } = vectorStorageId;
        public ApiKey? ApiKey { get; set; } =
            string.IsNullOrWhiteSpace(request.ApiKey) ? null : new ApiKey(request.ApiKey);

        public bool? IsActive { get; set; } = request.IsActive;
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