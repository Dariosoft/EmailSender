namespace Dariosoft.EmailSender.EndPoint.Abstraction.Contracts
{
    public interface IMessageEndPoint : IEndPoint
    {
        Task<Models.Result<Models.Common.BaseModel>> Create(Models.Message.CreateMessageModel model);

        Task<Models.Result> Update(Models.Message.UpdateMessageModel model);

        Task<Models.Result> Delete(string key);

        Task<Models.Result> TrySend(string key);

        Task<Models.Result> TryCancel(string key);

        Task<Models.ListResult<Models.Message.MessageModel>> List(Models.Common.ListQueryModel model);

        Task<Models.Result<Models.Message.MessageModel>> Get(string key);
    }
}
