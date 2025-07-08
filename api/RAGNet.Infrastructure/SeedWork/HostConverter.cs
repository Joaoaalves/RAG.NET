
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RAGNET.Domain.SharedKernel.VectorStorages.Hosts;

namespace RAGNET.Infrastructure.SeedWork
{
    public sealed class HostConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<Host?, string>(
        host => host != null ? host.Value : string.Empty,
        value => string.IsNullOrEmpty(value) ? null : new Host(value),
        mappingHints
    )
    { }
}