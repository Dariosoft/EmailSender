using Microsoft.AspNetCore.Http;

namespace Dariosoft.EmailSender.EndPoint.EndPoints
{
    public abstract class EndPoint
    {
        protected EndPoint(HttpContext context)
        {
            Request = context.Request;
            Response = context.Response;
        }

        protected HttpRequest Request { get; }

        protected HttpResponse Response { get; }
    }
}
