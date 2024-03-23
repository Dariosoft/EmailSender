namespace Dariosoft.EmailSender.Application.Concrete
{
    internal class AccountService(ServiceInjection injection) : Service(injection), IAccountService
    {
        public async Task<Reply<Core.Models.BaseModel>> Create(Request<Core.Models.CreateAccountModel> request)
        {
            #region Validation
            if (request.Payload is null)
                return Reply<Core.Models.BaseModel>.Fail(I18n.Messages.Error_InvalidInputData);

            if (!request.Payload.ClientKey.HasValue())
                return Reply<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Client));

            if (!request.Payload.HostKey.HasValue())
                return Reply<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Host));

            if (string.IsNullOrWhiteSpace(request.Payload.EmailAddress))
                return Reply<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.EmailAddress));

            if (string.IsNullOrWhiteSpace(request.Payload.Password))
                return Reply<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Password));
            #endregion

            #region Preparing prerequisite data
            var getClientResult = await GetClientId(request.Transform(request.Payload.ClientKey));

            if (!getClientResult.IsSuccessful)
                return Reply<Core.Models.BaseModel>.From(getClientResult);

            if (getClientResult.Data == null || getClientResult.Data == Guid.Empty)
                return Reply<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Host));

            var getHostResult = await GetHostId(request.Transform(request.Payload.HostKey));

            if (!getHostResult.IsSuccessful)
                return Reply<Core.Models.BaseModel>.From(getHostResult);

            if (getHostResult.Data == null || getHostResult.Data == Guid.Empty)
                return Reply<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Host));
            #endregion

            #region Preparing the model
            var emailAddress = request.Payload.EmailAddress.Trim().ToLower();

            var model = new Core.Models.AccountModel
            {
                Id = Guid.NewGuid(),
                Serial = 0,
                CreationTime = request.When,
                ClientId = getClientResult.Data.Value,
                HostId = getHostResult.Data.Value,
                Enabled = request.Payload.Enabled,
                EmailAddress = emailAddress,
                Password = Framework.Cryptography.PasswordEncoder.Encode(emailAddress, request.Payload.Password),
                DisplayName = string.IsNullOrWhiteSpace(request.Payload.DisplayName) ? null : request.Payload.DisplayName.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Payload.Description) ? null : request.Payload.Description.Trim(),
            }; 
            #endregion

            var reply = await accountRepository.Create(request.Transform(model));

            return Reply<Core.Models.BaseModel>.From(reply, () => model.TrimToBaseModel());
        }

        public async Task<Reply> Update(Request<Core.Models.UpdateAccountModel> request)
        {
            #region Validation
            if (request.Payload is null)
                return Reply.Fail(I18n.Messages.Error_InvalidInputData);
            
            if (!request.Payload.Key.HasValue())
                return Reply.Fail(I18n.Messages.Error_NoIdToUpdate);

            if (!request.Payload.ClientKey.HasValue())
                return Reply.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Client));

            if (!request.Payload.HostKey.HasValue())
                return Reply.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Host));

            if (string.IsNullOrWhiteSpace(request.Payload.EmailAddress))
                return Reply.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.EmailAddress));

            if (string.IsNullOrWhiteSpace(request.Payload.Password))
                return Reply.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Password));
            #endregion

            #region Preparing prerequisite data
            var getClientResult = await GetClientId(request.Transform(request.Payload.ClientKey));

            if (!getClientResult.IsSuccessful)
                return getClientResult.Trim();

            if (getClientResult.Data == null || getClientResult.Data == Guid.Empty)
                return Reply.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Host));

            var getHostResult = await GetHostId(request.Transform(request.Payload.HostKey));

            if (!getHostResult.IsSuccessful)
                return getHostResult.Trim();

            if (getHostResult.Data == null || getHostResult.Data == Guid.Empty)
                return Reply.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Host));
            #endregion

            #region Preparing the model
            var emailAddress = request.Payload.EmailAddress.Trim().ToLower();

            var model = new Core.Models.AccountModel
            {
                Id = request.Payload.Key.Id,
                Serial = request.Payload.Key.Serial,
                CreationTime = request.When,
                ClientId = getClientResult.Data.Value,
                HostId = getHostResult.Data.Value,
                Enabled = request.Payload.Enabled,
                EmailAddress = emailAddress,
                Password = Framework.Cryptography.PasswordEncoder.Encode(emailAddress, request.Payload.Password),
                DisplayName = string.IsNullOrWhiteSpace(request.Payload.DisplayName) ? null : request.Payload.DisplayName.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Payload.Description) ? null : request.Payload.Description.Trim(),
            };
            #endregion

            return await accountRepository.Update(request.Transform(model));
        }

        public Task<Reply> Delete(Request<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Reply.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Reply.Fail(I18n.Messages.Warning_RecordNotFound));

            return accountRepository.Delete(request);
        }

        public Task<Reply<Core.Models.AccountModel?>> Get(Request<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Reply<Core.Models.AccountModel?>.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Reply<Core.Models.AccountModel?>.Fail(I18n.Messages.Warning_RecordNotFound));

            return accountRepository.Get(request);
        }

        public Task<ListReply<Core.Models.AccountModel>> List(Request request)
        {
            return accountRepository.List(request);
        }

        public Task<Reply> SetAvailability(Request<Core.Models.SetAvailabilityModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Reply.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Reply.Fail(I18n.Messages.Warning_RecordNotFound));

            return accountRepository.SetAvailability(request);
        }


    }
}
