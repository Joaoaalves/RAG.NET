namespace RAGNET.Application.VectorStorages.Commands.UpdateVectorStorage
{
    public class UpdateVectorStorageRequest
    {
        public string? ApiKey { get; set; }
        public bool? IsActive { get; set; }
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