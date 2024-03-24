namespace Dariosoft.EmailSender.Core.Repositories
{
    public interface IAccountRepository : IRepository
    {
        Task<IResponse> Create(IRequest<Core.Models.AccountModel> request);

        Task<IResponse> Update(IRequest<Core.Models.AccountModel> request);

        Task<IResponse> Delete(IRequest<Core.Models.KeyModel> request);

        Task<IResponse<Core.Models.AccountModel?>> Get(IRequest<Core.Models.KeyModel> request);

        Task<IResponse> SetAvailability(IRequest<Core.Models.SetAvailabilityModel> request);

        Task<IListResponse<Core.Models.AccountModel>> List(IRequest request);
    }
}
