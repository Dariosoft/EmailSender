﻿namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models
{
    public class Result<T> : Result
    {
        public required T Data { get; init; }

        public static Result<T> Success(T data)
            => new Result<T> { IsSuccessful = true, Data = data };

        public new static Result<T> Fail(string message, string? code = null)
            => new Result<T> { IsSuccessful = false, Data = default, Messages = [new ResultMessage { Text = message, Code = code }] };
    }
}
