namespace Dariosoft.EmailSender.Application
{
    public interface IMessageService : IService
    {
        Task<Framework.Reply> Create(Framework.Request<Core.Models.MessageModel> request);

        Task<Framework.Reply> Update(Framework.Request<Core.Models.MessageModel> request);

        Task<Framework.Reply> Delete(Framework.Request<Core.Models.KeyModel> request);

        Task<Framework.Reply<Core.Models.MessageModel>> Get(Framework.Request<Core.Models.KeyModel> request);

        Task<Framework.ListReply<Core.Models.MessageModel>> List(Framework.Request request);

        Task<Framework.Reply> TrySend(Framework.Request<Core.Models.KeyModel> request);

        Task<Framework.Reply> TryCancel(Framework.Request<Core.Models.KeyModel> request);
    }
}

