namespace Dariosoft.Framework
{
    public interface IListResponse<T> : IResponse<IReadOnlyList<T>>
    {
        int PageNumber { get; }

        int PageSize { get; }

        int TotalItems { get; }
    }
}
