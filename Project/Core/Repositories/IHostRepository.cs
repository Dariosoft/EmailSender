namespace Dariosoft.EmailSender.Core.Repositories
{
    public interface IHostRepository : IRepository
    {
        Task Create(Models.HostModel model);
    }
}
