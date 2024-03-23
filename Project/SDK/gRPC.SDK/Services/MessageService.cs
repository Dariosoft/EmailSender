namespace Dariosoft.EmailSender.EndPoint.gRPC.SDK.Services
{
    class MessageService : ServiceBase<GrpcInterface.GrpcMessageEndPoint.GrpcMessageEndPointClient>, Abstraction.Contracts.IMessageEndPoint
    {
        public MessageService(IConnectionInfo connectionInfo)
            : base(connectionInfo)
        {
        }

        public async Task<Models.Result<Models.Common.BaseModel>> Create(Models.Message.CreateMessageModel model)
        {
            try
            {
                var reply = await _client.CreateAsync(ModelMapper.ToGrpcMessage_CreateRequestMessage(model));
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
                var reply = await _client.DeleteAsync(ModelMapper.ToGrpcMessage_DeleteRequestMessage(key));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.Result.Fail("Unexpected Error.");
            }
        }

        public async Task<Models.Result<Models.Message.MessageModel>> Get(string key)
        {
            try
            {
                var reply = await _client.GetAsync(ModelMapper.ToGrpcMessage_GetRequestMessage(key));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.Result<Models.Message.MessageModel>.Fail("Unexpected Error.");
            }
        }

        public async Task<Models.ListResult<Models.Message.MessageModel>> List(Models.Common.ListQueryModel model)
        {
            try
            {
                var reply = await _client.ListAsync(ModelMapper.ToGrpcMessage_ListRequestMessage(model));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.ListResult<Models.Message.MessageModel>.Fail("Unexpected Error.");
            }
        }

        public async Task<Models.Result> Update(Models.Message.UpdateMessageModel model)
        {
            try
            {
                var reply = await _client.UpdateAsync(ModelMapper.ToGrpcMessage_UpdateRequestMessage(model));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.Result.Fail("Unexpected Error.");
            }
        }

        public async Task<Models.Result> TrySend(string key)
        {
            try
            {
                var reply = await _client.TrySendAsync(ModelMapper.ToGrpcMessage_TrySendRequestMessage(key));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.Result.Fail("Unexpected Error.");
            }
        }

        public async Task<Models.Result> TryCancel(string key)
        {
            try
            {
                var reply = await _client.TryCancelAsync(ModelMapper.ToGrpcMessage_TryCancelRequestMessage(key));
                return ModelMapper.FromGrpc(reply);
            }
            catch (Exception e)
            {
                //TODO: Log
                return Models.Result.Fail("Unexpected Error.");
            }
        }

        protected override GrpcInterface.GrpcMessageEndPoint.GrpcMessageEndPointClient CreateClient(GrpcChannel channel)
            => new GrpcInterface.GrpcMessageEndPoint.GrpcMessageEndPointClient(channel);
    }
}


