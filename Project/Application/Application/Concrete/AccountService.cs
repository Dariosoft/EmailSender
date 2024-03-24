namespace Dariosoft.EmailSender.Application.Concrete
{
    internal class AccountService(ServiceInjection injection) : Service(injection), IAccountService
    {
        public async Task<IResponse<Core.Models.BaseModel>> Create(IRequest<Core.Models.CreateAccountModel> request)
        {
            #region Validation
            if (request.Payload is null)
                return Response<Core.Models.BaseModel>.Fail(I18n.Messages.Error_InvalidInputData);

            if (!request.Payload.ClientKey.HasValue())
                return Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Client));

            if (!request.Payload.HostKey.HasValue())
                return Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Host));

            if (string.IsNullOrWhiteSpace(request.Payload.EmailAddress))
                return Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.EmailAddress));

            if (string.IsNullOrWhiteSpace(request.Payload.Password))
                return Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Password));

            if(!request.Payload.EmailAddress.IsValidEmailAddress())
                return Response<Core.Models.BaseModel>.Fail(I18n.Messages.Error_InvalidEmailAddress);
            #endregion

            #region Preparing prerequisite data
            var getClientResult = await GetClientId(request.Transform(request.Payload.ClientKey));

            if (!getClientResult.IsSuccessful)
                return Response<Core.Models.BaseModel>.From(getClientResult);

            if (getClientResult.Data == null || getClientResult.Data == Guid.Empty)
                return Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Host));

            var getHostResult = await GetHostId(request.Transform(request.Payload.HostKey));

            if (!getHostResult.IsSuccessful)
                return Response<Core.Models.BaseModel>.From(getHostResult);

            if (getHostResult.Data == null || getHostResult.Data == Guid.Empty)
                return Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Host));
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
                Password = AccountPasswordEncoder.Instnace.Encode(emailAddress, plainPassword: request.Payload.Password),
                DisplayName = string.IsNullOrWhiteSpace(request.Payload.DisplayName) ? null : request.Payload.DisplayName.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Payload.Description) ? null : request.Payload.Description.Trim(),
            }; 
            #endregion

            var reply = await accountRepository.Create(request.Transform(model));

            return Response<Core.Models.BaseModel>.From(reply, () => model.TrimToBaseModel());
        }

        public async Task<IResponse> Update(IRequest<Core.Models.UpdateAccountModel> request)
        {
            #region Validation
            if (request.Payload is null)
                return Response.Fail(I18n.Messages.Error_InvalidInputData);
            
            if (!request.Payload.Key.HasValue())
                return Response.Fail(I18n.Messages.Error_NoIdToUpdate);

            if (!request.Payload.ClientKey.HasValue())
                return Response.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Client));

            if (!request.Payload.HostKey.HasValue())
                return Response.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Host));

            if (string.IsNullOrWhiteSpace(request.Payload.EmailAddress))
                return Response.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.EmailAddress));

            if (string.IsNullOrWhiteSpace(request.Payload.Password))
                return Response.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Password));

            if (!request.Payload.EmailAddress.IsValidEmailAddress())
                return Response<Core.Models.BaseModel>.Fail(I18n.Messages.Error_InvalidEmailAddress);
            #endregion

            #region Preparing prerequisite data
            var getClientResult = await GetClientId(request.Transform(request.Payload.ClientKey));

            if (!getClientResult.IsSuccessful)
                return getClientResult.Trim();

            if (getClientResult.Data == null || getClientResult.Data == Guid.Empty)
                return Response.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Host));

            var getHostResult = await GetHostId(request.Transform(request.Payload.HostKey));

            if (!getHostResult.IsSuccessful)
                return getHostResult.Trim();

            if (getHostResult.Data == null || getHostResult.Data == Guid.Empty)
                return Response.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Host));
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
                Password = AccountPasswordEncoder.Instnace.Encode(emailAddress, plainPassword: request.Payload.Password),
                DisplayName = string.IsNullOrWhiteSpace(request.Payload.DisplayName) ? null : request.Payload.DisplayName.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Payload.Description) ? null : request.Payload.Description.Trim(),
            };
            #endregion

            return await accountRepository.Update(request.Transform(model));
        }

        public Task<IResponse> Delete(IRequest<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Response.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Response.Fail(I18n.Messages.Warning_RecordNotFound));

            return accountRepository.Delete(request);
        }

        public Task<IResponse<Core.Models.AccountModel?>> Get(IRequest<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Response<Core.Models.AccountModel?>.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Response<Core.Models.AccountModel?>.Fail(I18n.Messages.Warning_RecordNotFound));

            return accountRepository.Get(request);
        }

        public Task<IListResponse<Core.Models.AccountModel>> List(IRequest request)
        {
            return accountRepository.List(request);
        }

        public Task<IResponse> SetAvailability(IRequest<Core.Models.SetAvailabilityModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Response.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Response.Fail(I18n.Messages.Warning_RecordNotFound));

            return accountRepository.SetAvailability(request);
        }


    }
}
