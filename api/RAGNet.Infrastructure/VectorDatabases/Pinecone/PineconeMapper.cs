using Pinecone;

namespace RAGNET.Infrastructure.VectorDatabases.Pinecone
{
    public static class PineconeMapper
    {
        private static readonly IEnumerable<string> AWSRegions = [
            "us-east-1",
            "us-west-2",
            "eu-west-1"
        ];

        private static readonly IEnumerable<string> GCPRegions = [
            "us-central-1",
            "europe-west4"
        ];

        private static readonly IEnumerable<string> AzureRegions = [
            "eastus2"
        ];

        public static ServerlessSpecCloud ToServerlessSpecCloud(this string cloud)
        {
            return cloud switch
            {
                "0" => ServerlessSpecCloud.Gcp,
                "1" => ServerlessSpecCloud.Aws,
                "2" => ServerlessSpecCloud.Azure,
                _ => throw new ArgumentOutOfRangeException($"Pinecone Serverless Spec Cloud {cloud} is not supported")
            };
        }

        public static string ToServerlessRegion(this string region, ServerlessSpecCloud cloud)
        {
            return cloud switch
            {
                ServerlessSpecCloud.Aws when AWSRegions.Contains(region) => region,
                ServerlessSpecCloud.Gcp when GCPRegions.Contains(region) => region,
                ServerlessSpecCloud.Azure when AzureRegions.Contains(region) => region,
                _ => throw new ArgumentOutOfRangeException(nameof(region), $"Region {region} is not supported for cloud {cloud}")
            };
        }

        public static PineconeIndexType ToIndexType(this string indexType)
        {
            return indexType.ToLowerInvariant() switch
            {
                "serverless" => PineconeIndexType.Serverless,
                "pod" => PineconeIndexType.Pod,
                "byoc" => PineconeIndexType.Byoc,
                _ => throw new ArgumentOutOfRangeException(nameof(indexType), $"Index Type {indexType} is not supported")
            };
        }
    }
}