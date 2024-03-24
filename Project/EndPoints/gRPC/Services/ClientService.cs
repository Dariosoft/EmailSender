using Dariosoft.EmailSender.EndPoint.Abstraction.GrpcInterface;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Dariosoft.EmailSender.EndPoint.gRPC.Services
{
    [Authorize(Roles = "SuperAdmin")]
    public class ClientService(Abstraction.Contracts.IClientEndPoint endpoint) : GrpcClientEndPoint.GrpcClientEndPointBase
    {
        public override Task<GrpcResult_BaseModel> Create(GrpcClientEndPoint_Create_RequestMessage request, ServerCallContext context)
        {
            var model = ModelMapper.Instance.FromGrpc(request.Model);

            var reply = endpoint.Create(model).Done(
                onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                onFailed: exp => GrpcResult_BaseModel.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcResult> Update(GrpcClientEndPoint_Update_RequestMessage request, ServerCallContext context)
        {
            var model = ModelMapper.Instance.FromGrpc(request.Model);

            var reply = endpoint.Update(model)
               .Done(
                   onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                   onFailed: exp => GrpcResult.UnExcpectedError()
               );

            return reply;
        }

        public override Task<GrpcResult> Delete(GrpcClientEndPoint_Delete_RequestMessage request, ServerCallContext context)
        {
            var reply = endpoint.Delete(request.Key)
                .Done(
                    onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                    onFailed: exp => GrpcResult.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcResult_ClientModel> Get(GrpcClientEndPoint_Get_RequestMessage request, ServerCallContext context)
        {
            var reply = endpoint.Get(request.Key)
                .Done(
                    onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                    onFailed: exp => GrpcResult_ClientModel.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcListResult_ClientModel> List(GrpcClientEndPoint_List_RequestMessage request, ServerCallContext context)
        {
            var model = ModelMapper.Instance.FromGrpc(request.Model);

            var reply = endpoint.List(model)
                .Done(
                    onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                    onFailed: exp => GrpcListResult_ClientModel.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcResult> SetAvailability(GrpcClientEndPoint_SetAvailability_RequestMessage request, ServerCallContext context)
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
