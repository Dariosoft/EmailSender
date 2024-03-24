using Dariosoft.EmailSender.EndPoint.Abstraction.GrpcInterface;
using System.Net.Mail;

namespace Dariosoft.EmailSender.EndPoint.gRPC.SDK
{
    internal class ModelMapper
    {
        #region Result
        public static Models.Reason FromGrpc(GrpcReason input)
            => new Models.Reason { Code = input.Code, Text = input.Text };

        public static Models.Common.BaseModel FromGrpc(GrpcModel_BaseModel input)
            => new Models.Common.BaseModel { Id = input.Id, Serial = input.Serial, CreationTime = input.CreationTime.ToDateTimeOffset() };

        public static Models.Result<Models.Common.BaseModel> FromGrpc(GrpcResult_BaseModel input)
            => new Models.Result<Models.Common.BaseModel>
            {
                IsSuccessful = input.IsSuccessful,
                Errors = input.Errors.Select(ModelMapper.FromGrpc).ToArray(),
                Warnings = input.Warnings.Select(ModelMapper.FromGrpc).ToArray(),
                Data = input.IsSuccessful && input.Data is not null ? FromGrpc(input.Data) : null
            };

        public static Models.Result FromGrpc(GrpcResult input)
            => new Models.Result
            {
                IsSuccessful = input.IsSuccessful,
                Errors = input.Errors.Select(ModelMapper.FromGrpc).ToArray(),
                Warnings = input.Warnings.Select(ModelMapper.FromGrpc).ToArray(),
            };
        #endregion

        #region Common
        public static GrpcModel_SetAvailibility ToGrpc(Models.Common.SetAvailabilityModel input)
            => new GrpcModel_SetAvailibility { Key = input.Key, Enabled = input.Enabled };

        public static GrpcModel_ListQuery ToGrpc(Models.Common.ListQueryModel input)
            => new GrpcModel_ListQuery
            {
                Page = input.Page,
                PageSize = input.PageSize,
                DescendingSort = input.DescendingSort,
                Query = input.Query ?? "",
                SortBy = input.SortBy ?? "",
                Parameters = { input.Parameters ?? new Dictionary<string, string>() }
            };
        #endregion

        #region Host
        public static Models.Host.HostModel FromGrpc(GrpcModel_Host input)
            => new Models.Host.HostModel
            {
                Id = input.Id,
                Serial = input.Serial,
                CreationTime = input.CreationTime.ToDateTimeOffset(),
                ClientId = Guid.TryParse(input.ClientId.Value, out var clientId) && clientId != Guid.Empty ? clientId : null,
                ClientName = input.ClientName,
                Enabled = input.Enabled,
                UseSsl = input.UseSsl,
                PortNumber = input.PortNumber,
                Address = input.Address,
                Description = input.Description,
            };

        public static Models.Result<Models.Host.HostModel> FromGrpc(GrpcResult_HostModel input)
            => new Models.Result<Models.Host.HostModel>
            {
                IsSuccessful = input.IsSuccessful,
                Errors = input.Errors.Select(ModelMapper.FromGrpc).ToArray(),
                Warnings = input.Warnings.Select(ModelMapper.FromGrpc).ToArray(),
                Data = input.IsSuccessful && input.Data is not null ? FromGrpc(input.Data) : null
            };

        public static Models.ListResult<Models.Host.HostModel> FromGrpc(GrpcListResult_HostModel input)
            => new Models.ListResult<Models.Host.HostModel>
            {
                IsSuccessful = input.IsSuccessful,
                Errors = input.Errors.Select(ModelMapper.FromGrpc).ToArray(),
                Warnings = input.Warnings.Select(ModelMapper.FromGrpc).ToArray(),
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalItems = input.TotalItems,
                Data = input.IsSuccessful && input.Data is not null ? input.Data.Select(FromGrpc).ToArray() : []
            };

        public static GrpcModel_CreateHost ToGrpc(Models.Host.CreateHostModel input)
            => new GrpcModel_CreateHost
            {
                ClientKey = input.ClientKey ?? "",
                Address = input.Address ?? "",
                Enabled = input.Enabled,
                PortNumber = input.PortNumber,
                UseSsl = input.UseSsl,
                Description = input.Description ?? ""
            };

        public static GrpcModel_UpdateHost ToGrpc(Models.Host.UpdateHostModel input)
            => new GrpcModel_UpdateHost
            {
                Key = input.Key ?? "",
                ClientKey = input.ClientKey ?? "",
                Address = input.Address ?? "",
                Enabled = input.Enabled,
                PortNumber = input.PortNumber,
                UseSsl = input.UseSsl,
                Description = input.Description ?? ""
            };

        public static GrpcInterface.GrpcHostEndPoint_Create_RequestMessage ToGrpcHost_CreateRequestMessage(Models.Host.CreateHostModel input)
            => new GrpcInterface.GrpcHostEndPoint_Create_RequestMessage { Model = ToGrpc(input) };

        public static GrpcInterface.GrpcHostEndPoint_Update_RequestMessage ToGrpcHost_UpdateRequestMessage(Models.Host.UpdateHostModel input)
           => new GrpcInterface.GrpcHostEndPoint_Update_RequestMessage { Model = ToGrpc(input) };

        public static GrpcInterface.GrpcHostEndPoint_Delete_RequestMessage ToGrpcHost_DeleteRequestMessage(string key)
            => new GrpcInterface.GrpcHostEndPoint_Delete_RequestMessage { Key = key };

        public static GrpcInterface.GrpcHostEndPoint_Get_RequestMessage ToGrpcHost_GetRequestMessage(string key)
            => new GrpcInterface.GrpcHostEndPoint_Get_RequestMessage { Key = key };

        public static GrpcInterface.GrpcHostEndPoint_List_RequestMessage ToGrpcHost_ListRequestMessage(Models.Common.ListQueryModel model)
            => new GrpcInterface.GrpcHostEndPoint_List_RequestMessage { Model = ToGrpc(model) };

        public static GrpcInterface.GrpcHostEndPoint_SetAvailability_RequestMessage ToGrpcHost_SetAvailabilityRequestMessage(Models.Common.SetAvailabilityModel model)
            => new GrpcInterface.GrpcHostEndPoint_SetAvailability_RequestMessage { Model = ToGrpc(model) };
        #endregion

        #region Account
        public static Models.Account.AccountModel FromGrpc(GrpcModel_Account input)
            => new Models.Account.AccountModel
            {
                Id = input.Id,
                Serial = input.Serial,
                CreationTime = input.CreationTime.ToDateTimeOffset(),
                ClientId = input.ClientId,
                ClientName = input.ClientName,
                Enabled = input.Enabled,
                DisplayName = input.DisplayName,
                Host = input.Host,
                HostId = input.HostId,
                EmailAddress = input.EmailAddress,
                Password = input.Password,
                Description = input.Description,
            };

        public static Models.Result<Models.Account.AccountModel> FromGrpc(GrpcResult_AccountModel input)
            => new Models.Result<Models.Account.AccountModel>
            {
                IsSuccessful = input.IsSuccessful,
                Errors = input.Errors.Select(ModelMapper.FromGrpc).ToArray(),
                Warnings = input.Warnings.Select(ModelMapper.FromGrpc).ToArray(),
                Data = input.IsSuccessful && input.Data is not null ? FromGrpc(input.Data) : null
            };

        public static Models.ListResult<Models.Account.AccountModel> FromGrpc(GrpcListResult_AccountModel input)
            => new Models.ListResult<Models.Account.AccountModel>
            {
                IsSuccessful = input.IsSuccessful,
                Errors = input.Errors.Select(ModelMapper.FromGrpc).ToArray(),
                Warnings = input.Warnings.Select(ModelMapper.FromGrpc).ToArray(),
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalItems = input.TotalItems,
                Data = input.IsSuccessful && input.Data is not null ? input.Data.Select(FromGrpc).ToArray() : []
            };

        public static GrpcModel_CreateAccount ToGrpc(Models.Account.CreateAccountModel input)
            => new GrpcModel_CreateAccount
            {
                ClientKey = input.ClientKey ?? "",
                HostKey = input.HostKey ?? "",
                EmailAddress = input.EmailAddress ?? "",
                DisplayName = input.DisplayName ?? "",
                Password = input.Password ?? "",
                Enabled = input.Enabled,
                Description = input.Description ?? ""
            };

        public static GrpcModel_UpdateAccount ToGrpc(Models.Account.UpdateAccountModel input)
            => new GrpcModel_UpdateAccount
            {
                Key = input.Key ?? "",
                ClientKey = input.ClientKey ?? "",
                HostKey = input.HostKey ?? "",
                EmailAddress = input.EmailAddress ?? "",
                DisplayName = input.DisplayName ?? "",
                Password = input.Password ?? "",
                Enabled = input.Enabled,
                Description = input.Description ?? "",
            };

        public static GrpcInterface.GrpcAccountEndPoint_Create_RequestMessage ToGrpcAccount_CreateRequestMessage(Models.Account.CreateAccountModel input)
            => new GrpcInterface.GrpcAccountEndPoint_Create_RequestMessage { Model = ToGrpc(input) };

        public static GrpcInterface.GrpcAccountEndPoint_Update_RequestMessage ToGrpcAccount_UpdateRequestMessage(Models.Account.UpdateAccountModel input)
           => new GrpcInterface.GrpcAccountEndPoint_Update_RequestMessage { Model = ToGrpc(input) };

        public static GrpcInterface.GrpcAccountEndPoint_Delete_RequestMessage ToGrpcAccount_DeleteRequestMessage(string key)
            => new GrpcInterface.GrpcAccountEndPoint_Delete_RequestMessage { Key = key };

        public static GrpcInterface.GrpcAccountEndPoint_Get_RequestMessage ToGrpcAccount_GetRequestMessage(string key)
            => new GrpcInterface.GrpcAccountEndPoint_Get_RequestMessage { Key = key };

        public static GrpcInterface.GrpcAccountEndPoint_List_RequestMessage ToGrpcAccount_ListRequestMessage(Models.Common.ListQueryModel model)
            => new GrpcInterface.GrpcAccountEndPoint_List_RequestMessage { Model = ToGrpc(model) };

        public static GrpcInterface.GrpcAccountEndPoint_SetAvailability_RequestMessage ToGrpcAccount_SetAvailabilityRequestMessage(Models.Common.SetAvailabilityModel model)
            => new GrpcInterface.GrpcAccountEndPoint_SetAvailability_RequestMessage { Model = ToGrpc(model) };
        #endregion

        #region MailAddress
        public static Models.Common.MailAddress FromGrpc(GrpcModel_MailAddress input)
            => new Models.Common.MailAddress { Address = input.Address, DisplayName = input.DisplayName };

        public static GrpcModel_MailAddress ToGrpc(Models.Common.MailAddress input)
            => new GrpcModel_MailAddress { Address = input.Address ?? "", DisplayName = input.DisplayName ?? "" };
        #endregion

        #region Message
        public static Models.Message.MessageModel FromGrpc(GrpcModel_Message input)
            => new Models.Message.MessageModel
            {
                Id = input.Id,
                Serial = input.Serial,
                CreationTime = input.CreationTime.ToDateTimeOffset(),
                Body = input.Body,
                Subject = input.Subject,
                BodyIsHtml = input.BodyIsHtml,
                NumberOfTries = short.TryParse(input.NumberOfTries.Value, out var numberOfTries) ? numberOfTries : (short)0,
                Priority = (MailPriority)input.Priority,
                Status = (Enums.MessageStatus)input.Status,
                SubjectIsHtml = input.SubjectIsHtml,
                Bcc = input.Bcc?.Select(FromGrpc).ToArray(),
                Cc = input.Cc?.Select(FromGrpc).ToArray(),
                ReplyTo = input.ReplyTo?.Select(FromGrpc).ToArray(),
                To = input.To?.Select(FromGrpc).ToArray() ?? [],
                From = input.From is null ? null : FromGrpc(input.From),
                Headers = input.Headers?.ToDictionary(e => e.Key, e => e.Value),
            };

        public static Models.Result<Models.Message.MessageModel> FromGrpc(GrpcResult_MessageModel input)
            => new Models.Result<Models.Message.MessageModel>
            {
                IsSuccessful = input.IsSuccessful,
                Errors = input.Errors.Select(ModelMapper.FromGrpc).ToArray(),
                Warnings = input.Warnings.Select(ModelMapper.FromGrpc).ToArray(),
                Data = input.IsSuccessful && input.Data is not null ? FromGrpc(input.Data) : null
            };

        public static Models.ListResult<Models.Message.MessageModel> FromGrpc(GrpcListResult_MessageModel input)
            => new Models.ListResult<Models.Message.MessageModel>
            {
                IsSuccessful = input.IsSuccessful,
                Errors = input.Errors.Select(ModelMapper.FromGrpc).ToArray(),
                Warnings = input.Warnings.Select(ModelMapper.FromGrpc).ToArray(),
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalItems = input.TotalItems,
                Data = input.IsSuccessful && input.Data is not null ? input.Data.Select(FromGrpc).ToArray() : []
            };

        public static GrpcModel_CreateMessage ToGrpc(Models.Message.CreateMessageModel input)
            => new GrpcModel_CreateMessage
            {
                SourceAccountKey = input.SourceAccountKey ?? "",
                From = input.From is null ? null : ToGrpc(input.From),
                IsDraft = input.IsDraft,
                SubjectIsHtml = input.SubjectIsHtml,
                Subject = input.Subject ?? "",
                BodyIsHtml = input.BodyIsHtml,
                Body = input.Body ?? "",
                Priority = (GrpcMailPriority)input.Priority,
                To = { input.To.Select(ToGrpc) },
                Cc = { input.Cc is null ? [] : input.Cc.Select(ToGrpc) },
                Bcc = { input.Bcc is null ? [] : input.Bcc.Select(ToGrpc) },
                ReplyTo = { input.ReplyTo is null ? [] : input.ReplyTo.Select(ToGrpc) },
                Headers = { input.Headers ?? new Dictionary<string, string>() },
            };

        public static GrpcModel_UpdateMessage ToGrpc(Models.Message.UpdateMessageModel input)
            => new GrpcModel_UpdateMessage
            {
                Key = input.Key ?? "",
                SourceAccountKey = input.SourceAccountKey ?? "",
                From = input.From is null ? null : ToGrpc(input.From),
                SubjectIsHtml = input.SubjectIsHtml,
                Subject = input.Subject ?? "",
                BodyIsHtml = input.BodyIsHtml,
                Body = input.Body ?? "",
                Priority = (GrpcMailPriority)input.Priority,
                To = { input.To.Select(ToGrpc) },
                Cc = { input.Cc is null ? [] : input.Cc.Select(ToGrpc) },
                Bcc = { input.Bcc is null ? [] : input.Bcc.Select(ToGrpc) },
                ReplyTo = { input.ReplyTo is null ? [] : input.ReplyTo.Select(ToGrpc) },
                Headers = { input.Headers ?? new Dictionary<string, string>() },
            };

        public static GrpcInterface.GrpcMessageEndPoint_Create_RequestMessage ToGrpcMessage_CreateRequestMessage(Models.Message.CreateMessageModel input)
            => new GrpcInterface.GrpcMessageEndPoint_Create_RequestMessage { Model = ToGrpc(input) };

        public static GrpcInterface.GrpcMessageEndPoint_Update_RequestMessage ToGrpcMessage_UpdateRequestMessage(Models.Message.UpdateMessageModel input)
           => new GrpcInterface.GrpcMessageEndPoint_Update_RequestMessage { Model = ToGrpc(input) };

        public static GrpcInterface.GrpcMessageEndPoint_Delete_RequestMessage ToGrpcMessage_DeleteRequestMessage(string key)
            => new GrpcInterface.GrpcMessageEndPoint_Delete_RequestMessage { Key = key ?? "" };

        public static GrpcInterface.GrpcMessageEndPoint_Get_RequestMessage ToGrpcMessage_GetRequestMessage(string key)
            => new GrpcInterface.GrpcMessageEndPoint_Get_RequestMessage { Key = key ?? "" };

        public static GrpcInterface.GrpcMessageEndPoint_List_RequestMessage ToGrpcMessage_ListRequestMessage(Models.Common.ListQueryModel model)
            => new GrpcInterface.GrpcMessageEndPoint_List_RequestMessage { Model = ToGrpc(model) };

        public static GrpcInterface.GrpcMessageEndPoint_TrySend_RequestMessage ToGrpcMessage_TrySendRequestMessage(string key)
            => new GrpcInterface.GrpcMessageEndPoint_TrySend_RequestMessage { Key = key ?? "" };

        public static GrpcInterface.GrpcMessageEndPoint_TryCancel_RequestMessage ToGrpcMessage_TryCancelRequestMessage(string key)
            => new GrpcInterface.GrpcMessageEndPoint_TryCancel_RequestMessage { Key = key ?? "" };
        #endregion
    }
}