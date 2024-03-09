using Dariosoft.EmailSender.EndPoint.Abstraction.Models;
using Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common;
using Dariosoft.EmailSender.EndPoint.Abstraction.Models.Host;
using Microsoft.AspNetCore.Mvc;


namespace Dariosoft.EmailSender.EndPoint.Api.Controllers
{
    [ApiController, Route("api/host")]
    public class HostController(Abstraction.Contracts.IHostEndPoint endPoint) : ControllerBase
    {

        [HttpPost("create")]
        public Task<Result<BaseModel>> Create([FromBody] CreateHostModel model)
            => endPoint.Create(model);

        [HttpPost("update")]
        public Task<Result> Update([FromBody] UpdateHostModel model)
            => endPoint.Update(model);

        [HttpDelete("delete/{key}")]
        public Task<Result> Delete([FromRoute] string key)
            => endPoint.Delete(key);

        [HttpGet("get/{key}")]
        public Task<Result<HostModel>> Get([FromRoute] string key)
            => endPoint.Get(key);

        [HttpGet("list")]
        public Task<ListResult<HostModel>> List([FromQuery] ListQueryModel model)
            => endPoint.List(model);

        [HttpPost("set-availability")]
        public Task<Result> SetAvailability([FromBody] SetAvailabilityModel model)
            => endPoint.SetAvailability(model);
    }
}
