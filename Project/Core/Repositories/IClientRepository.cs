namespace Dariosoft.EmailSender.Core.Repositories
{
    public interface IClientRepository : IRepository
    {
        Task<IResponse> Create(IRequest<Models.ClientModel> request);

        Task<IResponse> Update(IRequest<Core.Models.ClientModel> request);

        Task<IResponse> Delete(IRequest<Core.Models.KeyModel> request);

        Task<IResponse<Core.Models.ClientModel?>> Get(IRequest<Core.Models.KeyModel> request);

        Task<IListResponse<Core.Models.ClientModel>> List(IRequest request);

        Task<IResponse> SetAvailability(IRequest<Core.Models.SetAvailabilityModel> request);
    }
}
