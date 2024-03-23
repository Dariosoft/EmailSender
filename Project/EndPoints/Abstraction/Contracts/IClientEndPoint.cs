namespace Dariosoft.EmailSender.EndPoint.Abstraction.Contracts
{
    public interface IClientEndPoint : IEndPoint
    {
        Task<Models.Result<Models.Common.BaseModel>> Create(Models.Client.CreateClientModel model);

        Task<Models.Result> Delete(string key);

        Task<Models.Result> Update(Models.Client.UpdateClientModel model);

        Task<Models.Result> SetAvailability(Models.Common.SetAvailabilityModel model);

        Task<Models.ListResult<Models.Client.ClientModel>> List(Models.Common.ListQueryModel model);

        Task<Models.Result<Models.Client.ClientModel>> Get(string key);
    }
}
