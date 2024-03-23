namespace Dariosoft.EmailSender.Core.Repositories
{
    public interface IHostRepository : IRepository
    {
        Task<Reply> Create(Request<Models.HostModel> model);

        Task<Reply> Update(Request<Core.Models.HostModel> request);

        Task<Reply> Delete(Request<Core.Models.KeyModel> request);

        Task<Reply<Core.Models.HostModel?>> Get(Request<Core.Models.KeyModel> request);

        Task<Reply> SetAvailability(Request<Core.Models.SetAvailabilityModel> request);

        Task<ListReply<Core.Models.HostModel>> List(Request request);
    }
}
