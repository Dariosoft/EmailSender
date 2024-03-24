namespace Dariosoft.Framework
{
    public interface IResponse
    {
        bool IsSuccessful { get; }

        List<Reason> Errors { get; }

        List<Reason> Warnings { get; }
    }
}
