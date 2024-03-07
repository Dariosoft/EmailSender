namespace Dariosoft.EmailSender.EndPoint.Abstraction.Contracts
{
    public interface IAccountEndPoint: IEndPoint
    {
        Task<Models.Result<Models.Common.ModelCreationResult>> Create(Models.Account.CreateAccountModel model);

        Task<Models.Result> Delete(string key);

        Task<Models.Result> Update(Models.Account.UpdateAccountModel model);

        Task<Models.Result> SetAvailability(Models.Common.SetAvailabilityModel model);

        Task<Models.ListResult<Models.Account.AccountModel>> List(Models.Common.ListQueryModel model);

        Task<Models.Result<Models.Account.AccountModel>> Get(string key);
    }
}
