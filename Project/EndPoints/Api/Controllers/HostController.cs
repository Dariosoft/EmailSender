using Dariosoft.EmailSender.EndPoint.Abstraction.Models;
using Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common;
using Dariosoft.EmailSender.EndPoint.Abstraction.Models.Host;
using Microsoft.AspNetCore.Mvc;

namespace Dariosoft.EmailSender.EndPoint.Api.Controllers
{
    [ApiController, Route("api/host")]
    public class HostController : ControllerBase, Abstraction.Contracts.IHostEndPoint
    {
        [HttpPost("create")]
        public Task<Result<ModelCreationResult>> Create([FromBody]CreateHostModel model)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("delete/{key}")]
        public Task<Result> Delete([FromRoute]string key)
        {
            throw new NotImplementedException();
        }

        [HttpGet("get/{key}")]
        public Task<Result<HostModel>> Get([FromRoute] string key)
        {
            throw new NotImplementedException();
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
