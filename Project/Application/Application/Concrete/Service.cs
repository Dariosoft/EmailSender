using Dariosoft.EmailSender.Core.Repositories;

namespace Dariosoft.EmailSender.Application.Concrete
{
    class ServiceInjection
        (
        Core.Repositories.IClientRepository clientRepository,
        Core.Repositories.IHostRepository hostRepository,
        Core.Repositories.IAccountRepository accountRepository
        )
    {
        public Core.Repositories.IClientRepository ClientRepository { get; } = clientRepository;
        public Core.Repositories.IHostRepository HostRepository { get; } = hostRepository;
        public Core.Repositories.IAccountRepository AccountRepository { get; } = accountRepository;
    }

    abstract class Service(ServiceInjection injection)
    {
        protected readonly Core.Repositories.IClientRepository clientRepository = injection.ClientRepository;
        protected readonly Core.Repositories.IHostRepository hostRepository = injection.HostRepository;
        protected readonly Core.Repositories.IAccountRepository accountRepository = injection.AccountRepository;

        protected async Task<Reply<Guid?>> GetClientId(Request<Core.Models.KeyModel?> request)
        {
            if (request.Payload?.HasValue() == false)
                return Reply<Guid?>.Success(null);

            var reply = await clientRepository.Get(request!);

            return Reply<Guid?>.From(reply, () => reply.Data?.Id);
        }

        protected async Task<Reply<Guid?>> GetHostId(Request<Core.Models.KeyModel> request)
        {
            if (!request.Payload.HasValue())
                return Reply<Guid?>.Success(null);

            var reply = await hostRepository.Get(request!);

            return Reply<Guid?>.From(reply, () => reply.Data?.Id);
        }

        protected async Task<Reply<Guid?>> GetAccountId(Request<Core.Models.KeyModel> request)
        {
            if (!request.Payload.HasValue())
                return Reply<Guid?>.Success(null);

            var reply = await accountRepository.Get(request!);

            return Reply<Guid?>.From(reply, () => reply.Data?.Id);
        }
    }
}
