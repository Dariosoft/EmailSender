namespace Dariosoft.EmailSender.Core.Repositories
{
    public interface IClientRepository : IRepository
    {
        Task<Reply> Create(Request<Models.ClientModel> request);

        Task<Reply> Update(Request<Core.Models.ClientModel> request);

        Task<Reply> Delete(Request<Core.Models.KeyModel> request);

        Task<Reply<Core.Models.ClientModel?>> Get(Request<Core.Models.KeyModel> request);

        Task<ListReply<Core.Models.ClientModel>> List(Request request);

        Task<Reply> SetAvailability(Request<Core.Models.SetAvailabilityModel> request);
    }
}
