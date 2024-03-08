using Dariosoft.EmailSender.EndPoint.Abstraction.Models;
using Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common;
using Dariosoft.EmailSender.EndPoint.Abstraction.Models.Host;
using Dariosoft.EmailSender.EndPoint.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dariosoft.EmailSender.EndPoint.Api.Controllers
{
    [ApiController, Route("api/host")]
    public class HostController(Application.IHostService service)
        : ControllerBase, Abstraction.Contracts.IHostEndPoint
    {

        [HttpPost("create")]
        public Task<Result<ModelCreationResult?>> Create([FromBody] CreateHostModel model)
        {
            var payload = model.ToHostModel();
            var req = Request.Transform(payload);

            var reply = service.Create(req).Done(
                    onSuccess: res => new Result<ModelCreationResult?>
                    {
                        IsSuccessful = res.IsSuccessful,
                        Data = res.IsSuccessful ? new ModelCreationResult
                        {
                            CreationTime = req.When,
                            Key = payload.Id.ToString(),
                            Serial = payload.Serial
                        } : null,
                        Messages = res.Errors.Select(x => new ResultMessage { Text = x.Text, Code = x.Code }).ToArray()
                    },
                    onFailed: t => Result<ModelCreationResult?>.Fail("Unexpected error.")
                );

            return reply;
        }

        [HttpDelete("delete/{key}")]
        public Task<Result> Delete([FromRoute] string key)
        {
            var req = Request.Transform<Core.Models.KeyModel>(key);

            var resp = service.Delete(req).ContinueWith(task =>
            {
                return new Result
                {
                    IsSuccessful = task.Result.IsSuccessful,
                    Messages = task.Result.Errors.Select(x => new ResultMessage { Text = x.Text, Code = x.Code }).ToArray()
                };
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            return resp;
        }

        [HttpGet("get/{key}")]
        public Task<Result<HostModel?>> Get([FromRoute] string key)
        {
            var req = Request.Transform<Core.Models.KeyModel>(key);

            var resp = service.Get(req).ContinueWith(task =>
            {
                return new Result<HostModel?>
                {
                    IsSuccessful = task.Result.IsSuccessful,
                    Data = task.Result.Data is null ? null : new HostModel
                    {
                        Id = task.Result.Data.Id.ToString(),
                        Serial = task.Result.Data.Serial,
                        Address = task.Result.Data.Address,
                        PortNumber = task.Result.Data.PortNumber,
                        UseSsl = task.Result.Data.UseSsl,
                        Enabled = task.Result.Data.Enabled,
                        Description = task.Result.Data.Description,
                    },
                    Messages = task.Result.Errors.Select(x => new ResultMessage { Text = x.Text, Code = x.Code }).ToArray()
                };
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            return resp;
        }


        [HttpGet("list")]
        public Task<ListResult<HostModel>> List([FromQuery] ListQueryModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPost("set-availability")]
        public Task<Result> SetAvailability([FromBody] SetAvailabilityModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPost("update")]
        public Task<Result> Update([FromBody] UpdateHostModel model)
        {
            throw new NotImplementedException();
        }
    }
}
