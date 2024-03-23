namespace Dariosoft.EmailSender.Application
{
    public interface IPostmanService : IService
    {
        Task<Reply<bool>> SendMail(Request<Core.Models.MessageModel> request, CancellationToken cancellationToken);

        Reply ClearCache(Request request);
    }
}

