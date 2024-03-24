namespace Dariosoft.EmailSender.Application.Concrete
{
    internal class HostService(ServiceInjection injection) 
        : Service(injection), IHostService
    {
        public async Task<IResponse<Core.Models.BaseModel>> Create(IRequest<Core.Models.CreateHostModel> request)
        {
            #region Validation
            if (request.Payload is null)
                return Response<Core.Models.BaseModel>.Fail(I18n.Messages.Error_InvalidInputData);

            if (string.IsNullOrWhiteSpace(request.Payload.Address))
                return Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.HostAddress));

            if (request.Payload.PortNumber < 1)
                return Response<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.PortNumber));
            #endregion

            var clientId = request.User.Type == Framework.Types.UserType.SuperAdmin
                ? (Guid?)null
                : request.User.GetUserId();

            #region Preparing the model
            var model = new Core.Models.HostModel
            {
                Id = Guid.NewGuid(),
                Serial = 0,
                CreationTime = request.When,
                ClientId = clientId,
                Enabled = request.Payload.Enabled,
                Address = request.Payload.Address.Trim(),
                PortNumber = request.Payload.PortNumber,
                UseSsl = request.Payload.UseSsl,
                Description = string.IsNullOrWhiteSpace(request.Payload.Description) ? null : request.Payload.Description.Trim(),
            }; 
            #endregion

            var reply = await hostRepository.Create(request.Transform(model));

            return Response<Core.Models.BaseModel>.From(reply, () => model.TrimToBaseModel());
        }

        public async Task<IResponse> Update(IRequest<Core.Models.UpdateHostModel> request)
        {
            #region Validation
            if (request.Payload is null)
                return Response.Fail(I18n.Messages.Error_InvalidInputData);

            if (!request.Payload.Key.HasValue())
                return Response.Fail(I18n.Messages.Error_NoIdToUpdate);

            if (string.IsNullOrWhiteSpace(request.Payload.Address))
                return Response.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.HostAddress));

            if (request.Payload.PortNumber < 1)
                return Response.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.PortNumber));
            #endregion

            #region Preparing prerequisite data
            var getClientResult = await GetClientId(request.Transform(request.Payload.ClientKey));

            if (!getClientResult.IsSuccessful)
                return Response<Core.Models.BaseModel>.From(getClientResult);
            #endregion

            #region Preparing the model
            var model = new Core.Models.HostModel
            {
                Id = request.Payload.Key.Id,
                Serial = request.Payload.Key.Serial,
                CreationTime = request.When,
                ClientId = getClientResult.Data,
                Enabled = request.Payload.Enabled,
                Address = request.Payload.Address.Trim(),
                PortNumber = request.Payload.PortNumber,
                UseSsl = request.Payload.UseSsl,
                Description = string.IsNullOrWhiteSpace(request.Payload.Description) ? null : request.Payload.Description.Trim(),
            }; 
            #endregion

            return await hostRepository.Update(request.Transform(model));
        }

        public Task<IResponse> Delete(IRequest<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Response.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Response.Fail(I18n.Messages.Warning_RecordNotFound));

            return hostRepository.Delete(request);
        }

        public Task<IResponse<Core.Models.HostModel?>> Get(IRequest<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Response<Core.Models.HostModel?>.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Response<Core.Models.HostModel?>.Fail(I18n.Messages.Warning_RecordNotFound));

            return hostRepository.Get(request);
        }

        public Task<IListResponse<Core.Models.HostModel>> List(IRequest request)
        {
            return hostRepository.List(request);
        }

        public Task<IResponse> SetAvailability(IRequest<Core.Models.SetAvailabilityModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Response.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Response.Fail(I18n.Messages.Warning_RecordNotFound));

            return hostRepository.SetAvailability(request);
        }
    }
}

