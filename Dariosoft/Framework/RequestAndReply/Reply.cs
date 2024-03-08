namespace Dariosoft.Framework
{
    public class Reply
    {
        public required bool IsSuccessful { get; init; }

        public List<Reason> Errors { get; init; } = [];

        public List<Reason> Warnings { get; init; } = [];

        public static Reply Success()
            => new Reply { IsSuccessful = true };

        public static Reply SuccessWithWarning(string message, string? code = null)
            => new Reply { IsSuccessful = true, Warnings = [new Reason { Text = message, Code = code }] };

        public static Reply Fail(string message, string? code = null)
            => new Reply { IsSuccessful = false, Errors = [new Reason { Text = message, Code = code }] };

        public override string ToString() => IsSuccessful ? "Successful" : "Failure";
    }
}
