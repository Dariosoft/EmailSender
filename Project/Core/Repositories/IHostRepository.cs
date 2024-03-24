namespace Dariosoft.EmailSender.Core.Repositories
{
    public interface IHostRepository : IRepository
    {
        Task<IResponse> Create(IRequest<Models.HostModel> model);

        Task<IResponse> Update(IRequest<Core.Models.HostModel> request);

        Task<IResponse> Delete(IRequest<Core.Models.KeyModel> request);

        Task<IResponse<Core.Models.HostModel?>> Get(IRequest<Core.Models.KeyModel> request);

        Task<IResponse> SetAvailability(IRequest<Core.Models.SetAvailabilityModel> request);

        Task<IListResponse<Core.Models.HostModel>> List(IRequest request);
    }
}
