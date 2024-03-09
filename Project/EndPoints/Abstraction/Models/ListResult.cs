namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models
{
    public class ListResult<T> : Result
    {
        private int pageNumber = 1, pageSize = 10, totalItems = 0;

        public required IEnumerable<T> Data { get; init; }

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

        public static ListResult<T> Success(IEnumerable<T> data) => Success(data, data.Count());

        public static ListResult<T> Success(IEnumerable<T> data, int totalItems, int pageNumber = 1, int pageSize = 15)
            => new ListResult<T> { IsSuccessful = true, Data = data, TotalItems = totalItems, PageNumber = pageNumber, PageSize = pageSize };

        public static new ListResult<T> Fail(string message, string? code = null)
            => new ListResult<T> { IsSuccessful = false, Data = [], Errors = [new Reason { Text = message, Code = code }] };
    }
}
