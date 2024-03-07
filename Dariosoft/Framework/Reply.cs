namespace Dariosoft.Framework
{
    public class Reply
    {
        public required bool IsSuccessful { get; init; }

        public List<Reason> Reasons { get; init; } = [];

        public static Reply Success()
            => new Reply { IsSuccessful = true };

        public static Reply Fail(string message, string? code = null)
            => new Reply { IsSuccessful = false, Reasons = [new Reason { Text = message, Code = code }] };

        public override string ToString() => IsSuccessful ? "Successful" : "Failure";
    }
}
