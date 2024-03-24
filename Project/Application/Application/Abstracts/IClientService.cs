namespace Dariosoft.EmailSender.Application
{
    public interface IClientService : IService
    {
        Task<IResponse<Core.Models.BaseModel>> Create(IRequest<Core.Models.CreateClientModel> request);

        Task<IResponse> Update(IRequest<Core.Models.UpdateClientModel> request);

        Task<IResponse> Delete(IRequest<Core.Models.KeyModel> request);

        Task<IResponse<Core.Models.ClientModel?>> Get(IRequest<Core.Models.KeyModel> request);

        Task<IListResponse<Core.Models.ClientModel>> List(IRequest request);

        Task<IResponse> SetAvailability(IRequest<Core.Models.SetAvailabilityModel> request);
    }
}

