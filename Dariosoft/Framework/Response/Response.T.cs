namespace Dariosoft.Framework
{
    public class Response<T> : Response, IResponse<T>
    {
        public required T? Data { get; init; }

        public IResponse Trim()
            => new Response { IsSuccessful = this.IsSuccessful, Errors = this.Errors, Warnings = this.Warnings };
        
        public static IResponse<T> Success(T data)
            => new Response<T> { IsSuccessful = true, Data = data };
     
        public static IResponse<T> SuccessWithWarning(T data, string message, string? code = null)
            => new Response<T> { IsSuccessful = true, Data = data, Warnings = [new Reason { Text = message, Code = code }] };

        public new static IResponse<T> Fail(string message, string? code = null)
            => new Response<T> { IsSuccessful = false, Data = default, Errors = [new Reason { Text = message, Code = code }] };

        public static IResponse<T> From(IResponse other, Func<T?>? getData = null)
            => new Response<T> { IsSuccessful = other.IsSuccessful, Data = other.IsSuccessful && getData is not null ? getData() : default, Errors = other.Errors };
    }
}
