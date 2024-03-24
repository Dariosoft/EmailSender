using Dariosoft.Framework;
using System.Net.Mail;

namespace Dariosoft.EmailSender.PostmanWorkerService
{
    public class Worker(Application.IMessageService messageService) : BackgroundService
    {


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IResponse<bool> reply;
            Console.Clear();

            while (!stoppingToken.IsCancellationRequested)
            {
                LogInfo($@"{DateTime.UtcNow:yyyy-MMM-dd HH:mm:ss}\{Enum.GetName(currentPriority)}\{counter}> Trying to send...");
                reply = await messageService.TrySend(GetRequest());

                if (!reply.IsSuccessful)
                {
                    LogError($@"{DateTime.UtcNow:yyyy-MMM-dd HH:mm:ss}\{Enum.GetName(currentPriority)}\{counter}> ERROR! {reply.Errors.FirstOrDefault()}");
                    await Task.Delay(10_000);
                }
                else
                {
                    if (!reply.Data)
                    {
                        LogWarning($@"{DateTime.UtcNow:yyyy-MMM-dd HH:mm:ss}\{Enum.GetName(currentPriority)}\{counter}> WARNING! {reply.Errors.FirstOrDefault()}");
                        NextPriority();
                        await Task.Delay(5_000);
                    }
                    else
                    {
                        LogSuccess($@"{DateTime.UtcNow:yyyy-MMM-dd HH:mm:ss}\{Enum.GetName(currentPriority)}\{counter}> OK.");
                        UpdatePriority();
                    }
                }
            }
        }

        private int counter, max = 5;
        private MailPriority currentPriority = MailPriority.High;

        private IRequest<MailPriority> GetRequest()
        {
            return new Request<MailPriority>
            {
                When = DateTimeOffset.UtcNow,
                Where = "PostmanWorkerService.ExecuteAsync()",
                UserIP = Framework.Helpers.MiscHelper.Instance.GetLocalIP().ToString(),
                UserAgent = Environment.OSVersion.VersionString,
                Payload = currentPriority,
                User = new Framework.Auth.UserIdentity 
                { 
                    Id = Guid.Empty.ToString(),
                    Serial = 0,
                    Type = Framework.Types.UserType.System,
                    IsAuthenticated = true,
                    UserIsAnonymouse = false,
                    UserName = "PostmanWorkerService",
                    DisplayName = "Postman Worker Service"
                }
            };
        }

        private void UpdatePriority()
        {
            if (counter < max)
            {
                counter++;
                return;
            }

            NextPriority();
        }

        private void NextPriority()
        {
            counter = 0;

            switch (currentPriority)
            {
                case MailPriority.Normal:
                    currentPriority = MailPriority.Low;
                    max = 3;
                    break;
                case MailPriority.Low:
                    currentPriority = MailPriority.High;
                    max = 5;
                    break;
                case MailPriority.High:
                    currentPriority = MailPriority.Normal;
                    max = 1;
                    break;
            }
        }

        private void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void LogSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
            Console.ResetColor();
        }
    }
}
