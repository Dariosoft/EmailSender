namespace Dariosoft.Framework
{
    public class Response : IResponse
    {
        public required bool IsSuccessful { get; init; }

        public List<Reason> Errors { get; init; } = [];

        public List<Reason> Warnings { get; init; } = [];

        public static IResponse Success()
            => new Response { IsSuccessful = true };

        public static IResponse SuccessWithWarning(string message, string? code = null)
            => new Response { IsSuccessful = true, Warnings = [new Reason { Text = message, Code = code }] };

        public static IResponse Fail(string message, string? code = null)
            => new Response { IsSuccessful = false, Errors = [new Reason { Text = message, Code = code }] };

        public override string ToString() => IsSuccessful ? "Successful" : "Failure";
    }
}
