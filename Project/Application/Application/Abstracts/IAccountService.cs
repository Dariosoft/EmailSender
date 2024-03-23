namespace Dariosoft.EmailSender.Application
{
    public interface IAccountService : IService
    {
        Task<Framework.Reply<Core.Models.BaseModel>> Create(Framework.Request<Core.Models.CreateAccountModel> request);

        Task<Framework.Reply> Update(Framework.Request<Core.Models.UpdateAccountModel> request);

        Task<Framework.Reply> Delete(Framework.Request<Core.Models.KeyModel> request);

        Task<Framework.Reply<Core.Models.AccountModel?>> Get(Framework.Request<Core.Models.KeyModel> request);

        Task<Framework.ListReply<Core.Models.AccountModel>> List(Framework.Request request);

        Task<Framework.Reply> SetAvailability(Framework.Request<Core.Models.SetAvailabilityModel> request);
    }
}
