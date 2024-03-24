namespace Dariosoft.EmailSender.Application
{
    public interface IAccountService : IService
    {
        Task<Framework.IResponse<Core.Models.BaseModel>> Create(Framework.IRequest<Core.Models.CreateAccountModel> request);

        Task<Framework.IResponse> Update(Framework.IRequest<Core.Models.UpdateAccountModel> request);

        Task<Framework.IResponse> Delete(Framework.IRequest<Core.Models.KeyModel> request);

        Task<Framework.IResponse<Core.Models.AccountModel?>> Get(Framework.IRequest<Core.Models.KeyModel> request);

        Task<Framework.IListResponse<Core.Models.AccountModel>> List(Framework.IRequest request);

        Task<Framework.IResponse> SetAvailability(Framework.IRequest<Core.Models.SetAvailabilityModel> request);
    }
}
