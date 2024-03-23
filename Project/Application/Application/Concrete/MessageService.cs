namespace Dariosoft.EmailSender.Application.Concrete
{
    internal class MessageService(ServiceInjection injection, Core.Repositories.IMessageRepository repository) : Service(injection), IMessageService
    {
        public async Task<Reply<Core.Models.BaseModel>> Create(Request<Core.Models.CreateMessageModel> request)
        {
            #region Validation
            if (request.Payload is null)
                return Reply<Core.Models.BaseModel>.Fail(I18n.Messages.Error_InvalidInputData);

            if (!request.Payload.SourceAccountKey.HasValue())
                return Reply<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Sender));

            if (string.IsNullOrWhiteSpace(request.Payload.Subject))
                return Reply<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Subject));

            if (request.Payload.To.Length == 0)
                return Reply<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Recipient));
            #endregion

            #region Preparing prerequisite data
            var getSourceAccountResult = await GetAccountId(request.Transform(request.Payload.SourceAccountKey));

            if (!getSourceAccountResult.IsSuccessful)
                return Reply<Core.Models.BaseModel>.From(getSourceAccountResult);

            if (getSourceAccountResult.Data == null || getSourceAccountResult.Data == Guid.Empty)
                return Reply<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Sender));
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
                From = request.Payload.From,
                To = request.Payload.To,
                Cc = request.Payload.Cc,
                Bcc = request.Payload.Bcc,
                ReplyTo = request.Payload.ReplyTo,
                Headers = request.Payload.Headers,
            };

            var reply = await repository.Create(request.Transform(model));

            return Reply<Core.Models.BaseModel>.From(reply, () => model.TrimToBaseModel());
        }

        public async Task<Reply> Update(Request<Core.Models.UpdateMessageModel> request)
        {
            #region Validation
            if (request.Payload is null)
                return Reply.Fail(I18n.Messages.Error_InvalidInputData);

            if (!request.Payload.SourceAccountKey.HasValue())
                return Reply.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Sender));

            if (string.IsNullOrWhiteSpace(request.Payload.Subject))
                return Reply.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Subject));

            if (request.Payload.To.Length == 0)
                return Reply.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Recipient));
            #endregion

            #region Preparing prerequisite data
            var getSourceAccountResult = await GetAccountId(request.Transform(request.Payload.SourceAccountKey));

            if (!getSourceAccountResult.IsSuccessful)
                return getSourceAccountResult.Trim();

            if (getSourceAccountResult.Data == null || getSourceAccountResult.Data == Guid.Empty)
                return Reply.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Sender));
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
                From = request.Payload.From,
                To = request.Payload.To,
                Cc = request.Payload.Cc,
                Bcc = request.Payload.Bcc,
                ReplyTo = request.Payload.ReplyTo,
                Headers = request.Payload.Headers,
            };

            return await repository.Update(request.Transform(model));
        }

        public Task<Reply> Delete(Request<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Reply.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Reply.Fail(I18n.Messages.Warning_RecordNotFound));

            return repository.Delete(request);
        }

        public Task<Reply<Core.Models.MessageModel?>> Get(Request<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Reply<Core.Models.MessageModel?>.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Reply<Core.Models.MessageModel?>.Fail(I18n.Messages.Warning_RecordNotFound));

            return repository.Get(request);
        }

        public Task<ListReply<Core.Models.MessageModel>> List(Request request)
        {
            return repository.List(request);
        }

        public Task<Reply> TrySend(Request<Core.Models.KeyModel> request)
        {
            throw new NotImplementedException();
        }

        public Task<Reply> TryCancel(Request<Core.Models.KeyModel> request)
        {
            throw new NotImplementedException();
        }


    }
}
