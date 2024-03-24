namespace Dariosoft.EmailSender.Application
{
    public interface IPostmanService : IService
    {
        Task<IResponse<bool>> SendMail(IRequest<Core.Models.MessageModel> request, CancellationToken cancellationToken);

        IResponse ClearCache(IRequest request);
    }
}

