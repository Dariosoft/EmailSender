using Dariosoft.EmailSender.EndPoint.Abstraction.GrpcInterface;
using Grpc.Core;

namespace Dariosoft.EmailSender.EndPoint.gRPC.Services
{
    public class AccountService(Abstraction.Contracts.IAccountEndPoint endpoint) : GrpcAccountEndPoint.GrpcAccountEndPointBase
    {
        public override Task<GrpcResult_BaseModel> Create(GrpcAccountEndPoint_Create_RequestMessage request, ServerCallContext context)
        {
            var model = ModelMapper.Instance.FromGrpc(request.Model);

            var reply = endpoint.Create(model).Done(
                onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                onFailed: exp => GrpcResult_BaseModel.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcResult> Update(GrpcAccountEndPoint_Update_RequestMessage request, ServerCallContext context)
        {
            var model = ModelMapper.Instance.FromGrpc(request.Model);

            var reply = endpoint.Update(model)
               .Done(
                   onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                   onFailed: exp => GrpcResult.UnExcpectedError()
               );

            return reply;
        }

        public override Task<GrpcResult> Delete(GrpcAccountEndPoint_Delete_RequestMessage request, ServerCallContext context)
        {
            var reply = endpoint.Delete(request.Key)
                .Done(
                    onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                    onFailed: exp => GrpcResult.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcResult_AccountModel> Get(GrpcAccountEndPoint_Get_RequestMessage request, ServerCallContext context)
        {
            var reply = endpoint.Get(request.Key)
                .Done(
                    onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                    onFailed: exp => GrpcResult_AccountModel.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcListResult_AccountModel> List(GrpcAccountEndPoint_List_RequestMessage request, ServerCallContext context)
        {
            var model = ModelMapper.Instance.FromGrpc(request.Model);

            var reply = endpoint.List(model)
                .Done(
                    onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                    onFailed: exp => GrpcListResult_AccountModel.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcResult> SetAvailability(GrpcAccountEndPoint_SetAvailability_RequestMessage request, ServerCallContext context)
        {
            var model = ModelMapper.Instance.FromGrpc(request.Model);

            var reply = endpoint.SetAvailability(model)
               .Done(
                   onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                   onFailed: exp => GrpcResult.UnExcpectedError()
               );

            return reply;
        }
    }
}
