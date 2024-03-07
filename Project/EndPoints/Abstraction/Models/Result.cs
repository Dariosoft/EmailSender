﻿namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models
{
    public class Result
    {
        public required bool IsSuccessful { get; init; }

        public Reason[] Messages { get; init; } = [];

        public static Result Success() 
            => new Result { IsSuccessful = true };

        public static Result Fail(string message, string? code)
            => new Result { IsSuccessful = false, Messages = [new Reason { Text = message, Code = code }] };

        public override string ToString() => IsSuccessful ? "Successful" : "Failure";
    }
}
