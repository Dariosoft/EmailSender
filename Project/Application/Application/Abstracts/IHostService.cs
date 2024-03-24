namespace Dariosoft.EmailSender.Application
{

    public interface IHostService : IService
    {
        Task<Framework.IResponse<Core.Models.BaseModel>> Create(Framework.IRequest<Core.Models.CreateHostModel> request);

        Task<Framework.IResponse> Update(Framework.IRequest<Core.Models.UpdateHostModel> request);

        Task<Framework.IResponse> Delete(Framework.IRequest<Core.Models.KeyModel> request);

        Task<Framework.IResponse<Core.Models.HostModel?>> Get(Framework.IRequest<Core.Models.KeyModel> request);

        Task<Framework.IListResponse<Core.Models.HostModel>> List(Framework.IRequest request);

        Task<Framework.IResponse> SetAvailability(Framework.IRequest<Core.Models.SetAvailabilityModel> request);
    }
}

