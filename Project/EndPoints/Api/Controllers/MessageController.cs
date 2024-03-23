using Dariosoft.EmailSender.EndPoint.Abstraction.Models;
using Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common;
using Dariosoft.EmailSender.EndPoint.Abstraction.Models.Message;
using Microsoft.AspNetCore.Mvc;


namespace Dariosoft.EmailSender.EndPoint.Api.Controllers
{
    [ApiController, Route("api/message")]
    public class MessageController(Abstraction.Contracts.IMessageEndPoint endPoint) : ControllerBase
    {

        [HttpPost("create")]
        public Task<Result<BaseModel>> Create([FromBody] CreateMessageModel model)
            => endPoint.Create(model);

        [HttpDelete("delete/{key}")]
        public Task<Result> Delete([FromRoute] string key)
            => endPoint.Delete(key);

        [HttpGet("get/{key}")]
        public Task<Result<MessageModel>> Get([FromRoute] string key)
            => endPoint.Get(key);

        [HttpGet("list")]
        public Task<ListResult<MessageModel>> List([FromQuery] ListQueryModel model)
            => endPoint.List(model);

        [HttpDelete("send/{key}")]
        public Task<Result> TrySend([FromRoute] string key)
            => endPoint.TrySend(key);

        [HttpDelete("cancel/{key}")]
        public Task<Result> TryCancel([FromRoute] string key)
            => endPoint.TryCancel(key);

        [HttpPost("update")]
        public Task<Result> Update([FromBody] UpdateMessageModel model)
            => endPoint.Update(model);
    }
}
