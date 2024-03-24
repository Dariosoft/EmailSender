namespace Dariosoft.Framework
{
    public class Request<T> : Request, IRequest<T>
    {
        public required T Payload { get; init; }
    }
}
