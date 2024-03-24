using Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common;
using Dariosoft.EmailSender.EndPoint.Abstraction.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Dariosoft.EmailSender.EndPoint.EndPoints
{
    class HostEndPoint(IHttpContextAccessor contextAccessor, Application.IHostService service)
        : EndPoint(contextAccessor.HttpContext), Abstraction.Contracts.IHostEndPoint
    {
        public Task<Abstraction.Models.Result<BaseModel>> Create(Abstraction.Models.Host.CreateHostModel model)
        {
            var reply = service.Create(Request.Transform(model.Patch())).Done(
                    onSuccess: res => res.Transform(() => res.Data!.Unpatch()),
                    onFailed: exp => Result<BaseModel>.Fail(I18n.Messages.Error_UnexpectedError)
                );

            return reply;
        }

        public Task<Abstraction.Models.Result> Delete(string key)
        {
            return service.Delete(Request.Transform<Core.Models.KeyModel>(key))
                 .Done(
                 onSuccess: res => res.Transform(),
                 onFailed: exp => Abstraction.Models.Result.Fail(I18n.Messages.Error_UnexpectedError)
                 );
        }

        public Task<Abstraction.Models.Result> Update(Abstraction.Models.Host.UpdateHostModel model)
        {
            var reply = service.Update(Request.Transform(model.Patch())).Done(
                    onSuccess: res => res.Transform(),
                    onFailed: exp => Result<BaseModel>.Fail(I18n.Messages.Error_UnexpectedError)
                );

            return reply;
        }

        public Task<Abstraction.Models.Result> SetAvailability(Abstraction.Models.Common.SetAvailabilityModel model)
        {
            var reply = service.SetAvailability(Request.Transform(model.Patch())).Done(
                    onSuccess: res => res.Transform(),
                    onFailed: exp => Result<BaseModel>.Fail(I18n.Messages.Error_UnexpectedError)
                );

            return reply;
        }

        public Task<Abstraction.Models.ListResult<Abstraction.Models.Host.HostModel>> List(Abstraction.Models.Common.ListQueryModel model)
        {
            return service.List(Request.Transform(listQuery: model.Patch()))
                .Done(
                    onSuccess: res => res.ListTransform(() => res.Data.Select(e => e.Unpatch())),
                    onFailed: exp => ListResult<Abstraction.Models.Host.HostModel>.Fail(I18n.Messages.Error_UnexpectedError)
                );
        }

        public Task<Abstraction.Models.Result<Abstraction.Models.Host.HostModel>> Get(string key)
        {
            return service.Get(Request.Transform<Core.Models.KeyModel>(key))
                 .Done(
                    onSuccess: res => res.Transform(() => res.Data?.Unpatch()),
                    onFailed: exp => Result<Abstraction.Models.Host.HostModel>.Fail(I18n.Messages.Error_UnexpectedError)
                 );
        }
    }
}
