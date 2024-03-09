using Dariosoft.EmailSender.EndPoint.Abstraction.GrpcInterface;
using Grpc.Core;

namespace Dariosoft.EmailSender.EndPoint.gRPC.Services
{
    public class HostService(Abstraction.Contracts.IHostEndPoint endpoint) : Abstraction.GrpcInterface.GrpcHostEndPoint.GrpcHostEndPointBase
    {
        public override Task<GrpcResult_BaseModel> Create(GrpcHostEndPoint_Create_RequestMessage request, ServerCallContext context)
        {
            var model = ModelMapper.Instance.FromGrpc(request.Model);

            var reply = endpoint.Create(model).Done(
                onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                onFailed: exp => GrpcResult_BaseModel.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcResult> Update(GrpcHostEndPoint_Update_RequestMessage request, ServerCallContext context)
        {
            var model = ModelMapper.Instance.FromGrpc(request.Model);

            var reply = endpoint.Update(model)
               .Done(
                   onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                   onFailed: exp => GrpcResult.UnExcpectedError()
               );

            return reply;
        }

        public override Task<GrpcResult> Delete(GrpcHostEndPoint_Delete_RequestMessage request, ServerCallContext context)
        {
            var reply = endpoint.Delete(request.Key)
                .Done(
                    onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                    onFailed: exp => GrpcResult.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcResult_HostModel> Get(GrpcHostEndPoint_Get_RequestMessage request, ServerCallContext context)
        {
            var reply = endpoint.Get(request.Key)
                .Done(
                    onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                    onFailed: exp => GrpcResult_HostModel.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcListResult_HostModel> List(GrpcHostEndPoint_List_RequestMessage request, ServerCallContext context)
        {
            var model = ModelMapper.Instance.FromGrpc(request.Model);

            var reply = endpoint.List(model)
                .Done(
                    onSuccess: res => ModelMapper.Instance.ToGrpc(res),
                    onFailed: exp => GrpcListResult_HostModel.UnExcpectedError()
                );

            return reply;
        }

        public override Task<GrpcResult> SetAvailability(GrpcHostEndPoint_SetAvailability_RequestMessage request, ServerCallContext context)
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
