namespace Dariosoft.Framework
{
    public interface IResponse<T> : IResponse
    {
        T? Data { get; }

        IResponse Trim();
    }
}
