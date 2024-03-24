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

        protected async Task<IResponse<Guid?>> GetClientId(IRequest<Core.Models.KeyModel?> request)
        {
            if (request.Payload?.HasValue() == false)
                return Response<Guid?>.Success(null);

            var reply = await clientRepository.Get(request!);

            return Response<Guid?>.From(reply, () => reply.Data?.Id);
        }

        protected async Task<IResponse<Guid?>> GetHostId(IRequest<Core.Models.KeyModel> request)
        {
            if (!request.Payload.HasValue())
                return Response<Guid?>.Success(null);

            var reply = await hostRepository.Get(request!);

            return Response<Guid?>.From(reply, () => reply.Data?.Id);
        }

        protected async Task<IResponse<Guid?>> GetAccountId(IRequest<Core.Models.KeyModel> request)
        {
            if (!request.Payload.HasValue())
                return Response<Guid?>.Success(null);

            var reply = await accountRepository.Get(request!);

            return Response<Guid?>.From(reply, () => reply.Data?.Id);
        }

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
    }
}
