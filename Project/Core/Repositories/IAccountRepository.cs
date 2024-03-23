namespace Dariosoft.EmailSender.Core.Repositories
{
    public interface IAccountRepository : IRepository
    {
        Task<Reply> Create(Request<Core.Models.AccountModel> request);

        Task<Reply> Update(Request<Core.Models.AccountModel> request);

        Task<Reply> Delete(Request<Core.Models.KeyModel> request);

        Task<Reply<Core.Models.AccountModel?>> Get(Request<Core.Models.KeyModel> request);

        Task<Reply> SetAvailability(Request<Core.Models.SetAvailabilityModel> request);

        Task<ListReply<Core.Models.AccountModel>> List(Request request);
    }
}
