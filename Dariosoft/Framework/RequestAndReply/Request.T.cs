namespace Dariosoft.Framework
{
    public class Request<T> : Request
    {
        public required T Payload { get; init; }
    }
}
