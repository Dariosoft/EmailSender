namespace Dariosoft.Framework
{
    public class ListResponse<T> : Response, IListResponse<T>
    {
        private int pageNumber = 1, pageSize = 10, totalItems = 0;

        public required IReadOnlyList<T> Data { get; init; }

        public int PageNumber
        {
            get => pageNumber;
            init => pageNumber = value < 1 ? 1 : pageNumber;
        }

        public int PageSize
        {
            get => pageSize;
            init => pageSize = value < 1 ? 1 : pageSize;
        }

        public int TotalItems
        {
            get => totalItems;
            init => totalItems = value < 0 ? 0 : totalItems;
        }

        public IResponse Trim()
        => new Response { IsSuccessful = this.IsSuccessful, Errors = this.Errors, Warnings = this.Warnings };


        public static IListResponse<T> Success(IReadOnlyList<T> data) => Success(data, data.Count());

        public static IListResponse<T> Success(IReadOnlyList<T> data, int totalItems, int pageNumber = 1, int pageSize = 15)
            => new ListResponse<T> { IsSuccessful = true, Data = data, TotalItems = totalItems, PageNumber = pageNumber, PageSize = pageSize };

        public new static IListResponse<T> Fail(string message, string? code = null)
            => new ListResponse<T> { IsSuccessful = false, Data = [], Errors = [new Reason { Text = message, Code = code }] };

        public static IListResponse<T> From(IResponse other, Func<IReadOnlyList<T>>? getData = null)
            => new ListResponse<T> { IsSuccessful = other.IsSuccessful, Data = other.IsSuccessful && getData is not null ? getData() : [], Errors = other.Errors };

        //public static ListReply<T> From(Reply other)
        //    => new ListReply<T> { IsSuccessful = other.IsSuccessful, Data = [], Errors = other.Errors };
    }
}
