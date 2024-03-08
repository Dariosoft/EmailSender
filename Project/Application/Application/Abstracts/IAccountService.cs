namespace Dariosoft.EmailSender.Application
{
    public interface IAccountService : IService
    {
        Task<Framework.Reply> Create(Framework.Request<Core.Models.AccountModel> request);

        Task<Framework.Reply> Update(Framework.Request<Core.Models.AccountModel> request);

        Task<Framework.Reply> Delete(Framework.Request<Core.Models.KeyModel> request);

        Task<Framework.Reply<Core.Models.AccountModel>> Get(Framework.Request<Core.Models.KeyModel> request);

        Task<Framework.ListReply<Core.Models.AccountModel>> List(Framework.ListQueryModel request);

        Task<Framework.Reply> SetAvailability(Framework.Request<Core.Models.SetAvailabilityModel> request);
    }
}
