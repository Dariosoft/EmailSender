namespace Dariosoft.EmailSender.Application.Concrete
{
    internal class HostService(ServiceInjection injection) 
        : Service(injection), IHostService
    {
        public async Task<Reply<Core.Models.BaseModel>> Create(Request<Core.Models.CreateHostModel> request)
        {
            #region Validation
            if (request.Payload is null)
                return Reply<Core.Models.BaseModel>.Fail(I18n.Messages.Error_InvalidInputData);

            if (string.IsNullOrWhiteSpace(request.Payload.Address))
                return Reply<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.HostAddress));

            if (request.Payload.PortNumber < 1)
                return Reply<Core.Models.BaseModel>.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.PortNumber));
            #endregion

            #region Preparing prerequisite data
            var getClientResult = await GetClientId(request.Transform(request.Payload.ClientKey));

            if (!getClientResult.IsSuccessful)
                return Reply<Core.Models.BaseModel>.From(getClientResult);
            #endregion

            #region Preparing the model
            var model = new Core.Models.HostModel
            {
                Id = Guid.NewGuid(),
                Serial = 0,
                CreationTime = request.When,
                ClientId = getClientResult.Data,
                Enabled = request.Payload.Enabled,
                Address = request.Payload.Address.Trim(),
                PortNumber = request.Payload.PortNumber,
                UseSsl = request.Payload.UseSsl,
                Description = string.IsNullOrWhiteSpace(request.Payload.Description) ? null : request.Payload.Description.Trim(),
            }; 
            #endregion

            var reply = await hostRepository.Create(request.Transform(model));

            return Reply<Core.Models.BaseModel>.From(reply, () => model.TrimToBaseModel());
        }

        public async Task<Reply> Update(Request<Core.Models.UpdateHostModel> request)
        {
            #region Validation
            if (request.Payload is null)
                return Reply.Fail(I18n.Messages.Error_InvalidInputData);

            if (!request.Payload.Key.HasValue())
                return Reply.Fail(I18n.Messages.Error_NoIdToUpdate);

            if (string.IsNullOrWhiteSpace(request.Payload.Address))
                return Reply.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.HostAddress));

            if (request.Payload.PortNumber < 1)
                return Reply.Fail(I18n.MessagesX.Error_MissingRequiredField(I18n.Labels.PortNumber));
            #endregion

            #region Preparing prerequisite data
            var getClientResult = await GetClientId(request.Transform(request.Payload.ClientKey));

            if (!getClientResult.IsSuccessful)
                return Reply<Core.Models.BaseModel>.From(getClientResult);
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

        public Task<Reply> Delete(Request<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Reply.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Reply.Fail(I18n.Messages.Warning_RecordNotFound));

            return hostRepository.Delete(request);
        }

        public Task<Reply<Core.Models.HostModel?>> Get(Request<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Reply<Core.Models.HostModel?>.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Reply<Core.Models.HostModel?>.Fail(I18n.Messages.Warning_RecordNotFound));

            return hostRepository.Get(request);
        }

        public Task<ListReply<Core.Models.HostModel>> List(Request request)
        {
            return hostRepository.List(request);
        }

        public Task<Reply> SetAvailability(Request<Core.Models.SetAvailabilityModel> request)
        {
            if (request.Payload is null)
                return Task.FromResult(Reply.Fail(I18n.Messages.Error_InvalidInputData));

            if (!request.Payload.HasValue())
                return Task.FromResult(Reply.Fail(I18n.Messages.Warning_RecordNotFound));

            return hostRepository.SetAvailability(request);
        }
    }
}

