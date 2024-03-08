namespace Dariosoft.Framework
{
    public class Reply<T> : Reply
    {
        public required T? Data { get; init; }

        public static Reply<T> Success(T data)
            => new Reply<T> { IsSuccessful = true, Data = data };

        public static Reply<T> SuccessWithWarning(T data, string message, string? code = null)
            => new Reply<T> { IsSuccessful = true, Data = data, Warnings = [new Reason { Text = message, Code = code }] };

        public new static Reply<T> Fail(string message, string? code = null)
            => new Reply<T> { IsSuccessful = false, Data = default, Errors = [new Reason { Text = message, Code = code }] };

        public static Reply<T> From(Reply other, T? data = default)
            => new Reply<T> { IsSuccessful = other.IsSuccessful, Data = data, Errors = other.Errors };
    }
}
