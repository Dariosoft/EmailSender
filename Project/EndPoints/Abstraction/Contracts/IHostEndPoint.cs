namespace Dariosoft.EmailSender.EndPoint.Abstraction.Contracts
{
    public interface IHostEndPoint: IEndPoint
    {
        Task<Models.Result<Models.Common.ModelCreationResult>> Create(Models.Host.CreateHostModel model);

        Task<Models.Result> Delete(string key);

        Task<Models.Result> Update(Models.Host.UpdateHostModel model);

        Task<Models.Result> SetAvailability(Models.Common.SetAvailabilityModel model);

        Task<Models.ListResult<Models.Host.HostModel>> List(Models.Common.ListQueryModel model);

        Task<Models.Result<Models.Host.HostModel>> Get(string key);
    }
}
