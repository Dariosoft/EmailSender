using Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common;
using Dariosoft.EmailSender.EndPoint.Abstraction.Models;
using Microsoft.AspNetCore.Http;

namespace Dariosoft.EmailSender.EndPoint.EndPoints
{

    class MessageEndPoint(IHttpContextAccessor contextAccessor, Application.IMessageService service)
        : EndPoint(contextAccessor.HttpContext), Abstraction.Contracts.IMessageEndPoint
    {
        public Task<Abstraction.Models.Result<BaseModel>> Create(Abstraction.Models.Message.CreateMessageModel model)
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

        public Task<Abstraction.Models.Result> Update(Abstraction.Models.Message.UpdateMessageModel model)
        {
            var reply = service.Update(Request.Transform(model.Patch())).Done(
                    onSuccess: res => res.Transform(),
                    onFailed: exp => Result<BaseModel>.Fail(I18n.Messages.Error_UnexpectedError)
                );

            return reply;
        }

        public Task<Abstraction.Models.Result> TrySend(string key)
        {
            return service.TrySend(Request.Transform<Core.Models.KeyModel>(key))
                 .Done(
                 onSuccess: res => res.Transform(),
                 onFailed: exp => Abstraction.Models.Result.Fail(I18n.Messages.Error_UnexpectedError)
                 );
        }

        public Task<Abstraction.Models.Result> TryCancel(string key)
        {
            return service.TryCancel(Request.Transform<Core.Models.KeyModel>(key))
                 .Done(
                 onSuccess: res => res.Transform(),
                 onFailed: exp => Abstraction.Models.Result.Fail(I18n.Messages.Error_UnexpectedError)
                 );
        }

        public Task<Abstraction.Models.ListResult<Abstraction.Models.Message.MessageModel>> List(Abstraction.Models.Common.ListQueryModel model)
        {
            return service.List(Request.Transform(listQuery: model.Patch()))
                .Done(
                    onSuccess: res => res.ListTransform(() => res.Data.Select(e => e.Unpatch())),
                    onFailed: exp => ListResult<Abstraction.Models.Message.MessageModel>.Fail(I18n.Messages.Error_UnexpectedError)
                );
        }

        public Task<Abstraction.Models.Result<Abstraction.Models.Message.MessageModel>> Get(string key)
        {
            return service.Get(Request.Transform<Core.Models.KeyModel>(key))
                 .Done(
                    onSuccess: res => res.Transform(() => res.Data?.Unpatch()),
                    onFailed: exp => Result<Abstraction.Models.Message.MessageModel>.Fail(I18n.Messages.Error_UnexpectedError)
                 );
        }
    }
}
