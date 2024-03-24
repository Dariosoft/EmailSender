using Dariosoft.EmailSender.EndPoint.Abstraction.GrpcInterface;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Dariosoft.EmailSender.EndPoint.gRPC.Services
{
    [Authorize]
    public class MessageService(Abstraction.Contracts.IMessageEndPoint endpoint) : GrpcMessageEndPoint.GrpcMessageEndPointBase
    {
        public override Task<GrpcResult_BaseModel> Create(GrpcMessageEndPoint_Create_RequestMessage request, ServerCallContext context)
        {
            var model = ModelMapper.Instance.FromGrpc(request.Model);

            var reply = endpoint.Create(model).Done(
                onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                onFailed: exp => GrpcResult_BaseModel.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcResult> Update(GrpcMessageEndPoint_Update_RequestMessage request, ServerCallContext context)
        {
            var model = ModelMapper.Instance.FromGrpc(request.Model);

            var reply = endpoint.Update(model)
               .Done(
                   onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                   onFailed: exp => GrpcResult.UnExcpectedError()
               );

            return reply;
        }

        public override Task<GrpcResult> Delete(GrpcMessageEndPoint_Delete_RequestMessage request, ServerCallContext context)
        {
            var reply = endpoint.Delete(request.Key)
                .Done(
                    onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                    onFailed: exp => GrpcResult.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcResult_MessageModel> Get(GrpcMessageEndPoint_Get_RequestMessage request, ServerCallContext context)
        {
            var reply = endpoint.Get(request.Key)
                .Done(
                    onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                    onFailed: exp => GrpcResult_MessageModel.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcListResult_MessageModel> List(GrpcMessageEndPoint_List_RequestMessage request, ServerCallContext context)
        {
            var model = ModelMapper.Instance.FromGrpc(request.Model);
            
            var reply = endpoint.List(model)
                .Done(
                    onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                    onFailed: exp => GrpcListResult_MessageModel.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcResult> TrySend(GrpcMessageEndPoint_TrySend_RequestMessage request, ServerCallContext context)
        {

            var reply = endpoint.TrySend(request.Key)
                .Done(
                    onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                    onFailed: exp => GrpcResult.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcResult> TryCancel(GrpcMessageEndPoint_TryCancel_RequestMessage request, ServerCallContext context)
        {
            var reply = endpoint.TryCancel(request.Key)
                .Done(
                    onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                    onFailed: exp => GrpcResult.UnExcpectedError()
                );

            return reply;
        }
    }
}
