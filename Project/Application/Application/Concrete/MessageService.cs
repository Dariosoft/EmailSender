using Dariosoft.Framework;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Dariosoft.EmailSender.Application.Concrete
{
    internal class MessageService(
        ServiceInjection injection,
        IPostmanService postmanService,
        Core.Repositories.IMessageRepository repository) : Service(injection), IMessageService
    {
        public async Task<IResponse<Core.Models.BaseModel>> Create(IRequest<Core.Models.CreateMessageModel> request)
        {
            #region Validation
            if (request.Payload is null)
                return Response<Core.Models.BaseModel>.Fail(I18n.Messages.Error_InvalidInputData);

            if (!request.Payload.SourceAccountKey.HasValue())
                return Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Sender));

            if (string.IsNullOrWhiteSpace(request.Payload.Subject))
                return Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Subject));

            if (request.Payload.To.Length == 0)
                return Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Recipient));

            if (request.Payload.To.Any(t => !t.DisplayName.IsValidEmailAddress()))
                return Response<Core.Models.BaseModel>.Fail(I18n.Messages.Error_InvalidEmailAddress, "MailMessage.To");

            if (request.Payload.Cc?.Any(t => !t.DisplayName.IsValidEmailAddress()) == true)
                return Response<Core.Models.BaseModel>.Fail(I18n.Messages.Error_InvalidEmailAddress, "MailMessage.Cc");

            if (request.Payload.Bcc?.Any(t => !t.DisplayName.IsValidEmailAddress()) == true)
                return Response<Core.Models.BaseModel>.Fail(I18n.Messages.Error_InvalidEmailAddress, "MailMessage.Bcc");

            if (request.Payload.ReplyTo?.Any(t => !t.DisplayName.IsValidEmailAddress()) == true)
                return Response<Core.Models.BaseModel>.Fail(I18n.Messages.Error_InvalidEmailAddress, "MailMessage.ReplyTo");
            #endregion

            #region Preparing prerequisite data
            var getSourceAccountResult = await GetAccountId(request.Transform(request.Payload.SourceAccountKey));

            if (!getSourceAccountResult.IsSuccessful)
                return Response<Core.Models.BaseModel>.From(getSourceAccountResult);

            if (getSourceAccountResult.Data == null || getSourceAccountResult.Data == Guid.Empty)
                return Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Sender));
            #endregion

            var model = new Core.Models.MessageModel
            {
                Id = Guid.NewGuid(),
                Serial = 0,
                CreationTime = request.When,
                AccountId = getSourceAccountResult.Data.Value,
                Priority = request.Payload.Priority,
                Status = request.Payload.IsDraft ? Enums.MessageStatus.Draft : Enums.MessageStatus.Pending,
                LastStatusTime = request.When,
                Subject = request.Payload.Subject,
                SubjectIsHtml = request.Payload.SubjectIsHtml,
                Body = request.Payload.Body,
                BodyIsHtml = request.Payload.BodyIsHtml,
                NumberOfTries = 0,
                From = request.Payload.From,
                To = request.Payload.To,
                Cc = request.Payload.Cc,
                Bcc = request.Payload.Bcc,
                ReplyTo = request.Payload.ReplyTo,
                Headers = request.Payload.Headers,
            };

            var reply = await repository.Create(request.Transform(model));

            return Response<Core.Models.BaseModel>.From(reply, () => model.TrimToBaseModel());
        }

        public async Task<IResponse> Update(IRequest<Core.Models.UpdateMessageModel> request)
        {
            #region Validation
            if (request.Payload is null)
                return Response.Fail(I18n.Messages.Error_InvalidInputData);

            if (!request.Payload.SourceAccountKey.HasValue())
                return Response.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Sender));

            if (string.IsNullOrWhiteSpace(request.Payload.Subject))
                return Response.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Subject));

            if (request.Payload.To.Length == 0)
                return Response.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Recipient));
            #endregion

            #region Preparing prerequisite data
            var getSourceAccountResult = await GetAccountId(request.Transform(request.Payload.SourceAccountKey));

            if (!getSourceAccountResult.IsSuccessful)
                return getSourceAccountResult.Trim();

            if (getSourceAccountResult.Data == null || getSourceAccountResult.Data == Guid.Empty)
                return Response.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Sender));
            #endregion

            var model = new Core.Models.MessageModel
            {
                Id = request.Payload.Key.Id,
                Serial = request.Payload.Key.Serial,
                CreationTime = request.When,
                AccountId = getSourceAccountResult.Data.Value,
                Priority = request.Payload.Priority,
                Status = Enums.MessageStatus.Unknown,
                LastStatusTime = request.When,
                Subject = request.Payload.Subject,
                SubjectIsHtml = request.Payload.SubjectIsHtml,
                Body = request.Payload.Body,
                BodyIsHtml = request.Payload.BodyIsHtml,
                NumberOfTries = 0,
                From = request.Payload.From,
                To = request.Payload.To,
                Cc = request.Payload.Cc,
                Bcc = request.Payload.Bcc,
                ReplyTo = request.Payload.ReplyTo,
                Headers = request.Payload.Headers,
            };

            return await repository.Update(request.Transform(model));
        }

        public Task<IResponse> Delete(IRequest<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Response.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Response.Fail(I18n.Messages.Warning_RecordNotFound));

            return repository.Delete(request);
        }

        public Task<IResponse<Core.Models.MessageModel?>> Get(IRequest<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Response<Core.Models.MessageModel?>.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Response<Core.Models.MessageModel?>.Fail(I18n.Messages.Warning_RecordNotFound));

            return repository.Get(request);
        }

        public Task<IListResponse<Core.Models.MessageModel>> List(IRequest request)
        {
            return repository.List(request);
        }

        public async Task<IResponse<bool>> TrySend(IRequest<Core.Models.KeyModel> request)
        {
            var getResult = await repository.Get(request);

            if (!getResult.IsSuccessful) return Response<bool>.From(getResult);

            return await TrySend(request.Transform(getResult.Data));
        }

        public async Task<IResponse<bool>> TrySend(IRequest<MailPriority> request)
        {
            var getResult = await repository.GetItemToSend(request.Transform(new Core.Models.MessageGetHeadItem
            {
                Priority = request.Payload,
                MaxTry = GetMaxNumberOfTriesToSend(request.Payload)
            })); ;

            if (!getResult.IsSuccessful) return Response<bool>.From(getResult);

            return await TrySend(request.Transform(getResult.Data));
        }

        public Task<IResponse> TryCancel(IRequest<Core.Models.KeyModel> request)
        {
            throw new NotImplementedException();
        }

        private ushort GetMaxNumberOfTriesToSend(MailPriority priority)
        {
            return priority switch
            {
                MailPriority.High => 50,
                MailPriority.Low => 20,
                _ => 10
            };
        }
        
        private async Task<IResponse<bool>> TrySend(IRequest<Core.Models.MessageModel?> request)
        {
            if (request.Payload is null) return Response<bool>.SuccessWithWarning(false, I18n.Messages.Warning_RecordNotFound);

            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));

            var result = await postmanService.SendMail(request!, cancellationTokenSource.Token);

            await repository.SetStatus(request.Transform(new Core.Models.MessageStatusModel
            {
                Id = request.Payload.Id,
                Serial = request.Payload.Serial,
                Status = result.IsSuccessful ? Enums.MessageStatus.Sent : Enums.MessageStatus.Failed,
                Description = string.Join("\r\n", result.Errors.Select(e => e.ToString())),
                AddLog = true,
            }));

            return result;
        }
    }
}
