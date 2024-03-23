namespace Dariosoft.EmailSender.Application
{
    public interface IClientService : IService
    {
        Task<Reply<Core.Models.BaseModel>> Create(Request<Core.Models.CreateClientModel> request);

        Task<Reply> Update(Request<Core.Models.UpdateClientModel> request);

        Task<Reply> Delete(Request<Core.Models.KeyModel> request);

        Task<Reply<Core.Models.ClientModel?>> Get(Request<Core.Models.KeyModel> request);

        Task<ListReply<Core.Models.ClientModel>> List(Request request);

        Task<Reply> SetAvailability(Request<Core.Models.SetAvailabilityModel> request);
    }
}

