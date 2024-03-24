namespace Dariosoft.EmailSender.Core.Repositories
{
    public interface IMessageRepository : IRepository
    {
        Task<IResponse> Create(IRequest<Core.Models.MessageModel> request);

        Task<IResponse> Update(IRequest<Core.Models.MessageModel> request);

        Task<IResponse> Delete(IRequest<Core.Models.KeyModel> request);

        Task<IResponse<Core.Models.MessageModel?>> Get(IRequest<Core.Models.KeyModel> request);

        Task<IListResponse<Core.Models.MessageModel>> List(IRequest request);

        Task<IResponse<bool>> SetStatus(IRequest<Core.Models.MessageStatusModel> request);

        Task<IResponse<Core.Models.MessageModel?>> GetItemToSend(IRequest<Core.Models.MessageGetHeadItem> request);
    }
}
