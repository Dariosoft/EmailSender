namespace Dariosoft.EmailSender.EndPoint.gRPC.SDK.Services
{

    class HostService : ServiceBase<GrpcInterface.GrpcHostEndPoint.GrpcHostEndPointClient>, Abstraction.Contracts.IHostEndPoint
    {
        public HostService(IConnectionInfo connectionInfo)
            : base(connectionInfo)
        {
        }

        public async Task<Models.Result<Models.Common.BaseModel>> Create(Models.Host.CreateHostModel model)
        {
            try
            {
                var reply = await _client.CreateAsync(ModelMapper.ToGrpcHost_CreateRequestMessage(model));
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
                var reply = await _client.DeleteAsync(ModelMapper.ToGrpcHost_DeleteRequestMessage(key));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.Result.Fail("Unexpected Error.");
            }
        }

        public async Task<Models.Result<Models.Host.HostModel>> Get(string key)
        {
            try
            {
                var reply = await _client.GetAsync(ModelMapper.ToGrpcHost_GetRequestMessage(key));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.Result<Models.Host.HostModel>.Fail("Unexpected Error.");
            }
        }

        public async Task<Models.ListResult<Models.Host.HostModel>> List(Models.Common.ListQueryModel model)
        {
            try
            {
                var reply = await _client.ListAsync(ModelMapper.ToGrpcHost_ListRequestMessage(model));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.ListResult<Models.Host.HostModel>.Fail("Unexpected Error.");
            }
        }

        public async Task<Models.Result> SetAvailability(Models.Common.SetAvailabilityModel model)
        {
            try
            {
                var reply = await _client.SetAvailabilityAsync(ModelMapper.ToGrpcHost_SetAvailabilityRequestMessage(model));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.Result.Fail("Unexpected Error.");
            }
        }

        public async Task<Models.Result> Update(Models.Host.UpdateHostModel model)
        {
            try
            {
                var reply = await _client.UpdateAsync(ModelMapper.ToGrpcHost_UpdateRequestMessage(model));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.Result.Fail("Unexpected Error.");
            }
        }

        protected override GrpcInterface.GrpcHostEndPoint.GrpcHostEndPointClient CreateClient(GrpcChannel channel)
            => new GrpcInterface.GrpcHostEndPoint.GrpcHostEndPointClient(channel);
    }
}


