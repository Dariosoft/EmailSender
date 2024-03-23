namespace Dariosoft.EmailSender.EndPoint.gRPC.SDK.Services
{
    class AccountService : ServiceBase<GrpcInterface.GrpcAccountEndPoint.GrpcAccountEndPointClient>, Abstraction.Contracts.IAccountEndPoint
    {
        public AccountService(IConnectionInfo connectionInfo)
            : base(connectionInfo)
        {
        }

        public async Task<Models.Result<Models.Common.BaseModel>> Create(Models.Account.CreateAccountModel model)
        {
            try
            {
                var reply = await _client.CreateAsync(ModelMapper.ToGrpcAccount_CreateRequestMessage(model));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.Result<Models.Common.BaseModel>.Fail("Unexpected Error.");
            }
        }

        public async Task<Models.Result> Delete(string key)
        {
            try
            {
                var reply = await _client.DeleteAsync(ModelMapper.ToGrpcAccount_DeleteRequestMessage(key));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.Result.Fail("Unexpected Error.");
            }
        }

        public async Task<Models.Result<Models.Account.AccountModel>> Get(string key)
        {
            try
            {
                var reply = await _client.GetAsync(ModelMapper.ToGrpcAccount_GetRequestMessage(key));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.Result<Models.Account.AccountModel>.Fail("Unexpected Error.");
            }
        }

        public async Task<Models.ListResult<Models.Account.AccountModel>> List(Models.Common.ListQueryModel model)
        {
            try
            {
                var reply = await _client.ListAsync(ModelMapper.ToGrpcAccount_ListRequestMessage(model));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.ListResult<Models.Account.AccountModel>.Fail("Unexpected Error.");
            }
        }

        public async Task<Models.Result> SetAvailability(Models.Common.SetAvailabilityModel model)
        {
            try
            {
                var reply = await _client.SetAvailabilityAsync(ModelMapper.ToGrpcAccount_SetAvailabilityRequestMessage(model));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.Result.Fail("Unexpected Error.");
            }
        }

        public async Task<Models.Result> Update(Models.Account.UpdateAccountModel model)
        {
            try
            {
                var reply = await _client.UpdateAsync(ModelMapper.ToGrpcAccount_UpdateRequestMessage(model));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.Result.Fail("Unexpected Error.");
            }
        }

        protected override GrpcInterface.GrpcAccountEndPoint.GrpcAccountEndPointClient CreateClient(GrpcChannel channel)
            => new GrpcInterface.GrpcAccountEndPoint.GrpcAccountEndPointClient(channel);
    }
}
