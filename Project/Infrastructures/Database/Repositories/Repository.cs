namespace Dariosoft.EmailSender.Infrastructure.Database.Repositories
{

    internal abstract class Repository(RepositoryInjection injection)
    {
        protected DataSource.DbContext GetDbContext()
            => new DataSource.DbContext(injection.GetMainConnectionString());

        protected IResponse Fail(IRequest request, string where, Exception exception)
        {
            //TODO: Log
            return Response.Fail(I18n.Messages.Error_UnexpectedError);
        }

        protected IResponse<T> Fail<T>(IRequest request, string where, Exception exception)
        {
            //TODO: Log
            return Response<T>.Fail(I18n.Messages.Error_UnexpectedError);
        }

        protected IListResponse<T> ListFail<T>(IRequest request, string where, Exception exception)
        {
            //TODO: Log
            return ListResponse<T>.Fail(I18n.Messages.Error_UnexpectedError);
        }
    }
}
