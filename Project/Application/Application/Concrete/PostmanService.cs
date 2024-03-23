using System.Collections.Concurrent;
using System.Net.Mail;
using System.Text;

namespace Dariosoft.EmailSender.Application.Concrete
{
    class PostmanService(ServiceInjection injection)
        : Service(injection), IPostmanService
    {
        public async Task<Reply<bool>> SendMail(Request<Core.Models.MessageModel> request, CancellationToken cancellationToken)
        {
            if (request.Payload is null)
                return Reply<bool>.SuccessWithWarning(false, I18n.Messages.Warning_RecordNotFound);

            var account = await GetAccount(request);
            var host = await GetHost(request, account.HostId);
            Exception? exception = null;

            using (var smtp = new SmtpClient(host: host.Address, port: host.PortNumber))
            {
                try
                {
                    smtp.EnableSsl = host.UseSsl;
                    smtp.Credentials = new System.Net.NetworkCredential
                    {
                        UserName = account.EmailAddress,
                        Password = account.Password
                    };

                    var message = PrepareMailMessage(account, request.Payload);

                    await smtp.SendMailAsync(message, cancellationToken);
                }
                catch (Exception e)
                {
                    exception = e;
                }
            }

            return exception is null
                ? Reply<bool>.Success(true)
                : Reply<bool>.Fail(exception.Message, code: exception.GetType().FullName ?? exception.GetType().Name);  //Fail<bool>(request, nameof(SendMail), exception);
        }

        public Reply ClearCache(Request request)
        {
            _accounts.Clear();
            _hosts.Clear();
            return Reply.Success();
        }

        private MailMessage PrepareMailMessage(Core.Models.AccountModel account, Core.Models.MessageModel message)
        {
            var mail = new MailMessage
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.BodyIsHtml,
                Sender = new MailAddress(account.EmailAddress, account.DisplayName),
                From = message.From ?? new MailAddress(account.EmailAddress, account.DisplayName),
                Priority = message.Priority,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                BodyTransferEncoding = System.Net.Mime.TransferEncoding.Unknown,
            };

            for (var i = 0; i < message.To.Length; i++)
                mail.To.Add(message.To[i]);

            for (var i = 0; i < message.ReplyTo?.Length; i++)
                mail.To.Add(message.ReplyTo[i]);

            for (var i = 0; i < message.Cc?.Length; i++)
                mail.To.Add(message.Cc[i]);

            for (var i = 0; i < message.Bcc?.Length; i++)
                mail.To.Add(message.Bcc[i]);

            if (message.Headers is not null && message.Headers.Count > 0)
            {
                mail.HeadersEncoding = Encoding.UTF8;

                for (int i = 0; i < message.Headers.Count; i++)
                {
                    var kvp = message.Headers.ElementAt(i);
                    mail.Headers.Add(kvp.Key, kvp.Value);
                }
            }
            return mail;
        }

        private async Task<Core.Models.AccountModel> GetAccount(Request<Core.Models.MessageModel> request)
        {
            if (_accounts.TryGetValue(request.Payload.AccountId, out var account))
                return account;

            var result = await accountRepository.Get(request.Transform<Core.Models.KeyModel>(request.Payload.AccountId));

            if (result.Data is not null)
            {
                account = result.Data;
                account.Password = AccountPasswordEncoder.Instnace.Decode(account.EmailAddress, account.Password);
                _accounts.TryAdd(account.Id, account);
            }

            return account!;
        }

        private async Task<Core.Models.HostModel> GetHost(Request<Core.Models.MessageModel> request, Guid hostId)
        {
            if (_hosts.TryGetValue(hostId, out var host))
                return host;

            var result = await hostRepository.Get(request.Transform<Core.Models.KeyModel>(hostId));

            if (result.Data is not null)
            {
                host = result.Data;
                _hosts.TryAdd(host.Id, host);
            }

            return host!;
        }


        private readonly ConcurrentDictionary<Guid, Core.Models.AccountModel> _accounts = new();
        private readonly ConcurrentDictionary<Guid, Core.Models.HostModel> _hosts = new();
    }
}
