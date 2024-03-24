using Dariosoft.EmailSender.EndPoint.Abstraction.Models;
using Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common;
using Dariosoft.EmailSender.EndPoint.Abstraction.Models.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace Dariosoft.EmailSender.EndPoint.Api.Controllers
{
    [ApiController, Route("api/account"), Authorize]
    public class AccountController(Abstraction.Contracts.IAccountEndPoint endPoint) : ControllerBase
    {

        [HttpPost("create")]
        public Task<Result<BaseModel>> Create([FromBody] CreateAccountModel model)
            => endPoint.Create(model);

        [HttpDelete("delete/{key}")]
        public Task<Result> Delete([FromRoute] string key)
            => endPoint.Delete(key);

        [HttpGet("get/{key}")]
        public Task<Result<AccountModel>> Get([FromRoute] string key)
            => endPoint.Get(key);

        [HttpGet("list")]
        public Task<ListResult<AccountModel>> List([FromQuery] ListQueryModel model)
            => endPoint.List(model);

        [HttpPost("set-availability")]
        public Task<Result> SetAvailability([FromBody] SetAvailabilityModel model)
            => endPoint.SetAvailability(model);

        [HttpPost("update")]
        public Task<Result> Update([FromBody] UpdateAccountModel model)
            => endPoint.Update(model);
    }
}
