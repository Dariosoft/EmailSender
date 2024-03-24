namespace Dariosoft.EmailSender.Application.Concrete
{
    internal class ClientService(ServiceInjection injection) : Service(injection), IClientService
    {
        public Task<IResponse<Core.Models.BaseModel>> Create(IRequest<Core.Models.CreateClientModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Response<Core.Models.BaseModel>.Fail(I18n.Messages.Error_InvalidInputData));

            if (string.IsNullOrWhiteSpace(request.Payload.Name))
                return Task.FromResult(Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.Name)));

            if (string.IsNullOrWhiteSpace(request.Payload.AdminUserName))
                return Task.FromResult(Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.AdminUserName)));

            if (string.IsNullOrWhiteSpace(request.Payload.AdminPassword))
                return Task.FromResult(Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.AdminPassword)));

            var modelId = Guid.NewGuid();

            var model = new Core.Models.ClientModel
            {
                Id = modelId,
                Serial = 0,
                CreationTime = request.When,
                Enabled = request.Payload.Enabled,
                Name = request.Payload.Name.Trim(),
                AdminUserName = request.Payload.AdminUserName.Trim().ToLower(),
                AdminPassword = request.Payload.AdminPassword,
                Description = string.IsNullOrWhiteSpace(request.Payload.Description) ? null : request.Payload.Description.Trim(),
                ApiKey = Framework.Cryptography.GuidEncoder.Encode(modelId),
            };

            return clientRepository.Create(request.Transform(model))
                .Done<IResponse, IResponse<Core.Models.BaseModel>>(
                onSuccess: result =>
                {
                    return result.IsSuccessful
                        ? Response<Core.Models.BaseModel>.Success(model.TrimToBaseModel())
                        : Response<Core.Models.BaseModel>.From(result);
                },
                onFailed: exp => Response<Core.Models.BaseModel>.Fail(I18n.Messages.Error_UnexpectedError));
        }

        public Task<IResponse> Update(IRequest<Core.Models.UpdateClientModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Response.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.Key.HasValue())
                return Task.FromResult(Response.Fail(I18n.Messages.Error_NoIdToUpdate));


            if (string.IsNullOrWhiteSpace(request.Payload.Name))
                return Task.FromResult(Response.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.DisplayName)));

            if (string.IsNullOrWhiteSpace(request.Payload.AdminUserName))
                return Task.FromResult(Response.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.AdminUserName)));

            if (!request.Payload.Key.HasValue())
                return Task.FromResult(Response.Fail(I18n.Messages.Error_NoIdToUpdate));

            var model = new Core.Models.ClientModel
            {
                Id = request.Payload.Key.Id,
                Serial = request.Payload.Key.Serial,
                CreationTime = request.When,
                Enabled = request.Payload.Enabled,
                Name = request.Payload.Name.Trim(),
                AdminUserName = request.Payload.AdminUserName.Trim().ToLower(),
                AdminPassword = "",
                Description = string.IsNullOrWhiteSpace(request.Payload.Description) ? null : request.Payload.Description.Trim(),
            };

            return clientRepository.Update(request.Transform(model));
        }

        public Task<IResponse> Delete(IRequest<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Response.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Response.Fail(I18n.Messages.Warning_RecordNotFound));

            return clientRepository.Delete(request);
        }

        public Task<IResponse<Core.Models.ClientModel?>> Get(IRequest<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Response<Core.Models.ClientModel?>.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Response<Core.Models.ClientModel?>.Fail(I18n.Messages.Warning_RecordNotFound));

            return clientRepository.Get(request);
        }

        public Task<IListResponse<Core.Models.ClientModel>> List(IRequest request)
        {
            return clientRepository.List(request);
        }

        public Task<IResponse> SetAvailability(IRequest<Core.Models.SetAvailabilityModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Response.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Response.Fail(I18n.Messages.Warning_RecordNotFound));

            return clientRepository.SetAvailability(request);
        }
    }
}
