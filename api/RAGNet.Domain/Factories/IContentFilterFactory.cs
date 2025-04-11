using RAGNET.Domain.Entities;
using RAGNET.Domain.Services.Filter;

namespace RAGNET.Domain.Factories
{
    public interface IContentFilterFactory
    {
        IContentFilterService CreateContentFilter(Filter filter);
    }
}