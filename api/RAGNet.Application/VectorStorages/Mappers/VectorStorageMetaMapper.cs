using RAGNET.Domain.SharedKernel.Metas;

namespace RAGNET.Application.VectorStorages.Mappers
{
    public static class VectorStorageMetaMapper
    {
        public static Dictionary<string, string> ToDictionary(this IReadOnlyCollection<Meta> metas)
        {
            Dictionary<string, string> metaList = [];

            foreach (var meta in metas)
            {
                metaList.Add(meta.Key, meta.Value);
            }

            return metaList;
        }
    }
}