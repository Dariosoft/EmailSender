using Dariosoft.EmailSender.Core.Models;
using Microsoft.Extensions.Configuration;

namespace Dariosoft.EmailSender.Infrastructure.Database.Repositories
{

    internal abstract class Repository(RepositoryInjection injection)
    {
        protected DataSource.DbContext GetDbContext()
            => new DataSource.DbContext(injection.GetMainConnectionString());

        protected Reply Fail(Request request, string where, Exception exception)
        {
            //TODO: Log
            return Reply.Fail(I18n.Messages.Error_UnexpectedError);
        }

        protected Reply<T> Fail<T>(Request request, string where, Exception exception)
        {
            //TODO: Log
            return Reply<T>.Fail(I18n.Messages.Error_UnexpectedError);
        }

        protected ListReply<T> ListFail<T>(Request request, string where, Exception exception)
        {
            //TODO: Log
            return ListReply<T>.Fail(I18n.Messages.Error_UnexpectedError);
        }
    }
}
