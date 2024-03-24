using System.Net.Mail;

namespace Dariosoft.EmailSender.Application
{
    public interface IMessageService : IService
    {
        Task<Framework.IResponse<Core.Models.BaseModel>> Create(Framework.IRequest<Core.Models.CreateMessageModel> request);

        Task<Framework.IResponse> Update(Framework.IRequest<Core.Models.UpdateMessageModel> request);

        Task<Framework.IResponse> Delete(Framework.IRequest<Core.Models.KeyModel> request);

        Task<Framework.IResponse<Core.Models.MessageModel?>> Get(Framework.IRequest<Core.Models.KeyModel> request);

        Task<Framework.IListResponse<Core.Models.MessageModel>> List(Framework.IRequest request);

        Task<Framework.IResponse<bool>> TrySend(Framework.IRequest<Core.Models.KeyModel> request);

        Task<IResponse<bool>> TrySend(IRequest<MailPriority> request);

        Task<Framework.IResponse> TryCancel(Framework.IRequest<Core.Models.KeyModel> request);
    }
}

