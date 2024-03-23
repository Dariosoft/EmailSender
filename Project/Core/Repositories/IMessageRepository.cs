namespace Dariosoft.EmailSender.Core.Repositories
{
    public interface IMessageRepository : IRepository
    {
        Task<Reply> Create(Request<Core.Models.MessageModel> request);

        Task<Reply> Update(Request<Core.Models.MessageModel> request);

        Task<Reply> Delete(Request<Core.Models.KeyModel> request);

        Task<Reply<Core.Models.MessageModel?>> Get(Request<Core.Models.KeyModel> request);

        Task<ListReply<Core.Models.MessageModel>> List(Request request);
    }
}
