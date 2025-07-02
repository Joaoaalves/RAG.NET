namespace RAGNET.Application.Shared
{
    public record PagedResult<T>(List<T> Items, int TotalCount);
}