namespace Dariosoft.EmailSender.Application
{
    public interface IHostService : IService
    {
        Task<Framework.Reply> Create(Framework.Request<Core.Models.HostModel> request);

        Task<Framework.Reply> Update(Framework.Request<Core.Models.HostModel> request);

        Task<Framework.Reply> Delete(Framework.Request<Core.Models.KeyModel> request);

        Task<Framework.Reply<Core.Models.HostModel>> Get(Framework.Request<Core.Models.KeyModel> request);

        Task<Framework.ListReply<Core.Models.HostModel>> List(Framework.Request request);

        Task<Framework.Reply> SetAvailability(Framework.Request<Core.Models.SetAvailabilityModel> request);
    }
}

