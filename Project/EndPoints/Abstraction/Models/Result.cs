namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models
{
    public class Result
    {
        public required bool IsSuccessful { get; init; }

        public Reason[] Errors { get; init; } = [];

        public Reason[] Warnings { get; init; } = [];

        public static Result Success()
            => new Result { IsSuccessful = true };

        public static Result Fail(string message, string? code = null)
            => new Result { IsSuccessful = false, Errors = [new Reason { Text = message, Code = code }] };

        public override string ToString() => IsSuccessful ? "Successful" : "Failure";
    }
}
