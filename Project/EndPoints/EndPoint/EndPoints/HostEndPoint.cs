using Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common;
using Dariosoft.EmailSender.EndPoint.Abstraction.Models;
using Microsoft.AspNetCore.Http;

namespace Dariosoft.EmailSender.EndPoint.EndPoints
{

    class HostEndPoint(IHttpContextAccessor contextAccessor, Application.IHostService service)
        : EndPoint(contextAccessor.HttpContext), Abstraction.Contracts.IHostEndPoint
    {
        public Task<Abstraction.Models.Result<Abstraction.Models.Common.BaseModel>> Create(Abstraction.Models.Host.CreateHostModel model)
        {
            var req = Request.Transform(model.Patch());

            var reply = service.Create(req).Done(
                    onSuccess: res => res.Transform(() => req.Payload.ToModelCreationResult()),
                    onFailed: exp => Result<BaseModel>.Fail("Unexpected error.")
                );

            return reply;
        }

        public Task<Abstraction.Models.Result> Delete(string key)
        {
            throw new NotImplementedException();
        }


        public Task<Abstraction.Models.Result> Update(Abstraction.Models.Host.UpdateHostModel model)
        {
            throw new NotImplementedException();
        }

        public Task<Abstraction.Models.Result> SetAvailability(Abstraction.Models.Common.SetAvailabilityModel model)
        {
            throw new NotImplementedException();
        }

        public Task<Abstraction.Models.ListResult<Abstraction.Models.Host.HostModel>> List(Abstraction.Models.Common.ListQueryModel model)
        {
            throw new NotImplementedException();
        }

        public Task<Abstraction.Models.Result<Abstraction.Models.Host.HostModel>> Get(string key)
        {
            throw new NotImplementedException();
        }
    }
}
