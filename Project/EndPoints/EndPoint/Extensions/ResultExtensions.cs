namespace Dariosoft.EmailSender.EndPoint
{
    public static class ResultExtensions
    {
        public static Abstraction.Models.Result Transform<TReply>(this TReply reply)
           where TReply : Framework.IResponse
           => new Abstraction.Models.Result
           {
               IsSuccessful = reply.IsSuccessful,
               Errors = reply.Errors.Select(x => new Abstraction.Models.Reason { Text = x.Text, Code = x.Code }).ToArray(),
               Warnings = reply.Warnings.Select(x => new Abstraction.Models.Reason { Text = x.Text, Code = x.Code }).ToArray(),
           };

        public static Abstraction.Models.Result<T> Transform<T, TReply>(this TReply reply, Func<T?> getData)
           where TReply : Framework.IResponse
           => new Abstraction.Models.Result<T>
           {
               IsSuccessful = reply.IsSuccessful,
               Errors = reply.Errors.Select(x => new Abstraction.Models.Reason { Text = x.Text, Code = x.Code }).ToArray(),
               Warnings = reply.Warnings.Select(x => new Abstraction.Models.Reason { Text = x.Text, Code = x.Code }).ToArray(),
               Data = reply.IsSuccessful ? getData() : default,
           };

        public static Abstraction.Models.ListResult<T> ListTransform<T, TReply>(this Framework.IListResponse<TReply> reply, Func<IEnumerable<T>> getData)
           => new Abstraction.Models.ListResult<T>
           {
               IsSuccessful = reply.IsSuccessful,
               Errors = reply.Errors.Select(x => new Abstraction.Models.Reason { Text = x.Text, Code = x.Code }).ToArray(),
               Warnings = reply.Warnings.Select(x => new Abstraction.Models.Reason { Text = x.Text, Code = x.Code }).ToArray(),
               Data = reply.IsSuccessful ? getData() : [],
               PageNumber = reply.PageNumber,
               PageSize = reply.PageSize,
               TotalItems = reply.TotalItems
           };

        
    }
}
