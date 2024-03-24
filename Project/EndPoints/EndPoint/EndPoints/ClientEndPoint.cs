using Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common;
using Dariosoft.EmailSender.EndPoint.Abstraction.Models;
using Microsoft.AspNetCore.Http;

namespace Dariosoft.EmailSender.EndPoint.EndPoints
{
    class ClientEndPoint(IHttpContextAccessor contextAccessor, Application.IClientService service)
        : EndPoint(contextAccessor.HttpContext!), Abstraction.Contracts.IClientEndPoint
    {
        public Task<Result<BaseModel>> Create(Abstraction.Models.Client.CreateClientModel model)
        {
            var reply = service.Create(Request.Transform(model.Patch())).Done(
                    onSuccess: res => res.Transform(() => res.Data!.Unpatch() ) ,
                    onFailed: exp => Result<BaseModel>.Fail(I18n.Messages.Error_UnexpectedError)
                );

            return reply;
        }

        public Task<Result> Delete(string key)
        {
            return service.Delete(Request.Transform<Core.Models.KeyModel>(key))
                 .Done(
                 onSuccess: res => res.Transform(),
                 onFailed: exp => Result.Fail(I18n.Messages.Error_UnexpectedError)
                 );
        }

        public Task<Result> Update(Abstraction.Models.Client.UpdateClientModel model)
        {
            var reply = service.Update(Request.Transform(model.Patch())).Done(
                    onSuccess: res => res.Transform(),
                    onFailed: exp => Result<BaseModel>.Fail(I18n.Messages.Error_UnexpectedError)
                );

            return reply;
        }

        public Task<Result> SetAvailability(SetAvailabilityModel model)
        {
            return service.SetAvailability(Request.Transform(model.Patch()))
                 .Done(
                 onSuccess: res => res.Transform(),
                 onFailed: exp => Result.Fail(I18n.Messages.Error_UnexpectedError)
                 );
        }

        public Task<ListResult<Abstraction.Models.Client.ClientModel>> List(Abstraction.Models.Common.ListQueryModel model)
        {
            return service.List(Request.Transform(listQuery: model.Patch()))
                .Done(
                    onSuccess: res => res.ListTransform(() => res.Data.Select(e => e.Unpatch())),
                    onFailed: exp => ListResult<Abstraction.Models.Client.ClientModel>.Fail(I18n.Messages.Error_UnexpectedError)
                );
        }

        public Task<Result<Abstraction.Models.Client.ClientModel>> Get(string key)
        {
            return service.Get(Request.Transform<Core.Models.KeyModel>(key))
                 .Done(
                 onSuccess: res => res.Transform(() => res.Data?.Unpatch()),
                 onFailed: exp => Result<Abstraction.Models.Client.ClientModel>.Fail(I18n.Messages.Error_UnexpectedError)
                 );
        }
    }
}
