namespace Dariosoft.Framework
{
    public interface IRequest<T> : IRequest
    {
        T Payload { get; }
    }
}
