using Dariosoft.Framework;

namespace Dariosoft.EmailSender.EndPoint.gRPC
{
    public class ModelMapper
    {
        private ModelMapper() { }

        private static readonly Lazy<ModelMapper> _lazy = new Lazy<ModelMapper>(() => new ModelMapper());

        public static ModelMapper Instance => _lazy.Value;

        //---- FromGrpc ↓ ---------------------------
        public Abstraction.Models.Common.SetAvailabilityModel FromGrpc(GrpcModel_SetAvailibility input)
           => new Abstraction.Models.Common.SetAvailabilityModel
           {
               Key = input.Key,
               Enabled = input.Enabled,
           };

        public Abstraction.Models.Common.ListQueryModel FromGrpc(GrpcModel_ListQuery input)
            => new Abstraction.Models.Common.ListQueryModel
            {
                Page = input.Page,
                PageSize = input.PageSize,
                Query = input.Query,
                SortBy = input.SortBy,
                DescendingSort = input.DescendingSort,
                Parameters = input.Parameters.ToDictionary(e => e.Key, e => e.Value)
            };

        //---------- Client ↓ -----------------------
        public Abstraction.Models.Client.CreateClientModel FromGrpc(GrpcModel_CreateClient input)
            => new Abstraction.Models.Client.CreateClientModel
            {
                Name = input.Name,
                Enabled = input.Enabled,
                Description = input.Description,
            };

        public Abstraction.Models.Client.UpdateClientModel FromGrpc(GrpcModel_UpdateClient input)
            => new Abstraction.Models.Client.UpdateClientModel
            {
                Key = input.Key,
                Enabled = input.Enabled,
                Name = input.Name,
                Description = input.Description,
            };


        //---------- Host ↓ -------------------------
        public Abstraction.Models.Host.CreateHostModel FromGrpc(GrpcModel_CreateHost input)
            => new Abstraction.Models.Host.CreateHostModel
            {
                Address = input.Address,
                Enabled = input.Enabled,
                PortNumber = input.PortNumber,
                UseSsl = input.UseSsl,
                Description = input.Description,
            };

        public Abstraction.Models.Host.UpdateHostModel FromGrpc(GrpcModel_UpdateHost input)
            => new Abstraction.Models.Host.UpdateHostModel
            {
                Key = input.Key,
                Address = input.Address,
                Enabled = input.Enabled,
                PortNumber = input.PortNumber,
                UseSsl = input.UseSsl,
                Description = input.Description,
            };


        //---------- Account ↓ ----------------------
        public Abstraction.Models.Account.CreateAccountModel FromGrpc(GrpcModel_CreateAccount input)
            => new Abstraction.Models.Account.CreateAccountModel
            {
                HostKey = input.HostKey,
                Enabled = input.Enabled,
                DisplayName = input.DisplayName,
                EmailAddress = input.Username,
                Password = input.Password,
                Description = input.Description,
            };

        public Abstraction.Models.Account.UpdateAccountModel FromGrpc(GrpcModel_UpdateAccount input)
            => new Abstraction.Models.Account.UpdateAccountModel
            {
                AccountKey = input.AccountKey,
                HostKey = input.HostKey,
                Enabled = input.Enabled,
                DisplayName = input.DisplayName,
                EmailAddress = input.Username,
                Password = input.Password,
                Description = input.Description,
            };


        //---------- Mail Address ↓ ----------
        public Abstraction.Models.Common.MailAddress FromGrpc(GrpcModel_MailAddress input)
            => new Abstraction.Models.Common.MailAddress { Address = input.Address, DisplayName = input.DisplayName };

        //---------- Message ↓ ----------
        public Abstraction.Models.Message.CreateMessageModel FromGrpc(GrpcModel_CreateMessage input)
            => new Abstraction.Models.Message.CreateMessageModel
            {
                SourceAccountKey = input.SenderAccountKey,
                IsDraft = input.IsDraft,
                Subject = input.Subject,
                SubjectIsHtml = input.SubjectIsHtml,
                Body = input.Body,
                BodyIsHtml = input.BodyIsHtml,
                To = input.To.Select(FromGrpc).ToArray(),
                From = FromGrpc(input.From),
                Cc = input.Cc.Select(FromGrpc).ToArray(),
                Bcc = input.Bcc.Select(FromGrpc).ToArray(),
                ReplyTo = input.ReplyTo.Select(FromGrpc).ToArray(),
                Headers = input.Headers,
                Priority = (System.Net.Mail.MailPriority)input.Priority,
            };

        public Abstraction.Models.Message.UpdateMessageModel FromGrpc(GrpcModel_UpdateMessage input)
            => new Abstraction.Models.Message.UpdateMessageModel
            {
                Key = input.MessageKey,
                SourceAccountKey = input.SenderAccountKey,
                Subject = input.Subject,
                SubjectIsHtml = input.SubjectIsHtml,
                Body = input.Body,
                BodyIsHtml = input.BodyIsHtml,
                To = input.To.Select(FromGrpc).ToArray(),
                From = FromGrpc(input.From),
                Cc = input.Cc.Select(FromGrpc).ToArray(),
                Bcc = input.Bcc.Select(FromGrpc).ToArray(),
                ReplyTo = input.ReplyTo.Select(FromGrpc).ToArray(),
                Headers = input.Headers,
                Priority = (System.Net.Mail.MailPriority)input.Priority,
            };


        //---- ToGrpc ↓ -----------------------------
        public GrpcReason ToGrpcReason(Abstraction.Models.Reason reason)
            => new GrpcReason { Code = reason.Code ?? "", Text = reason.Text ?? "" };

        public IEnumerable<GrpcReason> ToGrpcReasonArray(IEnumerable<Abstraction.Models.Reason> reasons)
            => reasons.Select(ToGrpcReason);

        public GrpcResult ToGrpc(Abstraction.Models.Result input)
            => new GrpcResult
            {
                IsSuccessful = input.IsSuccessful,
                Errors = { ModelMapper.Instance.ToGrpcReasonArray(input.Errors) },
                Warnings = { ModelMapper.Instance.ToGrpcReasonArray(input.Warnings) },
            };

        public GrpcResult_BaseModel ToGrpc(Abstraction.Models.Result<Abstraction.Models.Common.BaseModel> input)
            => new GrpcResult_BaseModel
            {
                IsSuccessful = input.IsSuccessful,
                Errors = { ModelMapper.Instance.ToGrpcReasonArray(input.Errors) },
                Warnings = { ModelMapper.Instance.ToGrpcReasonArray(input.Warnings) },
                Data = input.IsSuccessful ? ToGrpc(input.Data!) : null,
            };

        public GrpcModel_BaseModel ToGrpc(Abstraction.Models.Common.BaseModel input)
            => new GrpcModel_BaseModel
            {
                Id = input.Id,
                Serial = input.Serial,
                CreationTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(input.CreationTime),
            };

        public GrpcModel_Client ToGrpc(Abstraction.Models.Client.ClientModel input)
            => new GrpcModel_Client
            {
                Id = input.Id,
                Serial = input.Serial,
                CreationTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(input.CreationTime),
                Name = input.Name,
                Description = input.Description,
                AccessKey = input.AccessKey,
                Enabled = input.Enabled,
            };

        public GrpcResult_ClientModel ToGrpc(Abstraction.Models.Result<Abstraction.Models.Client.ClientModel> input)
            => new GrpcResult_ClientModel
            {
                IsSuccessful = input.IsSuccessful,
                Errors = { ModelMapper.Instance.ToGrpcReasonArray(input.Errors) },
                Warnings = { ModelMapper.Instance.ToGrpcReasonArray(input.Warnings) },
                Data = input.IsSuccessful ? ToGrpc(input.Data!) : null,
            };

        public GrpcListResult_ClientModel ToGrpc(Abstraction.Models.ListResult<Abstraction.Models.Client.ClientModel> input)
            => new GrpcListResult_ClientModel
            {
                IsSuccessful = input.IsSuccessful,
                Errors = { ModelMapper.Instance.ToGrpcReasonArray(input.Errors) },
                Warnings = { ModelMapper.Instance.ToGrpcReasonArray(input.Warnings) },
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalItems = input.TotalItems,
                Data = { input.IsSuccessful ? input.Data.Select(ToGrpc) : [] }
            };

        public GrpcModel_Host ToGrpc(Abstraction.Models.Host.HostModel input)
            => new GrpcModel_Host
            {
                Id = input.Id,
                Serial = input.Serial,
                CreationTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(input.CreationTime),
                Address = input.Address,
                Description = input.Description,
                PortNumber = input.PortNumber,
                UseSsl = input.UseSsl,
                Enabled = input.Enabled,
            };

        public GrpcResult_HostModel ToGrpc(Abstraction.Models.Result<Abstraction.Models.Host.HostModel> input)
            => new GrpcResult_HostModel
            {
                IsSuccessful = input.IsSuccessful,
                Errors = { ModelMapper.Instance.ToGrpcReasonArray(input.Errors) },
                Warnings = { ModelMapper.Instance.ToGrpcReasonArray(input.Warnings) },
                Data = input.IsSuccessful ? ToGrpc(input.Data!) : null,
            };

        public GrpcListResult_HostModel ToGrpc(Abstraction.Models.ListResult<Abstraction.Models.Host.HostModel> input)
            => new GrpcListResult_HostModel
            {
                IsSuccessful = input.IsSuccessful,
                Errors = { ModelMapper.Instance.ToGrpcReasonArray(input.Errors) },
                Warnings = { ModelMapper.Instance.ToGrpcReasonArray(input.Warnings) },
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalItems = input.TotalItems,
                Data = { input.IsSuccessful ? input.Data.Select(ToGrpc) : [] }
            };

        public GrpcModel_Account ToGrpc(Abstraction.Models.Account.AccountModel input)
            => new GrpcModel_Account
            {
                Id = input.Id,
                Serial = input.Serial,
                HostId = input.HostId,
                CreationTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(input.CreationTime),
                DisplayName = input.DisplayName,
                Username = input.EmailAddress,
                Password = input.Password,
                Description = input.Description,
                Enabled = input.Enabled,
            };

        public GrpcResult_AccountModel ToGrpc(Abstraction.Models.Result<Abstraction.Models.Account.AccountModel> input)
            => new GrpcResult_AccountModel
            {
                IsSuccessful = input.IsSuccessful,
                Errors = { ModelMapper.Instance.ToGrpcReasonArray(input.Errors) },
                Warnings = { ModelMapper.Instance.ToGrpcReasonArray(input.Warnings) },
                Data = input.IsSuccessful ? ToGrpc(input.Data!) : null,
            };

        public GrpcListResult_AccountModel ToGrpc(Abstraction.Models.ListResult<Abstraction.Models.Account.AccountModel> input)
            => new GrpcListResult_AccountModel
            {
                IsSuccessful = input.IsSuccessful,
                Errors = { ModelMapper.Instance.ToGrpcReasonArray(input.Errors) },
                Warnings = { ModelMapper.Instance.ToGrpcReasonArray(input.Warnings) },
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalItems = input.TotalItems,
                Data = { input.IsSuccessful ? input.Data.Select(ToGrpc) : [] }
            };

        public GrpcModel_MailAddress ToGrpc(Abstraction.Models.Common.MailAddress input)
            => new GrpcModel_MailAddress { Address = input.Address, DisplayName = input.DisplayName };

        public GrpcModel_Message ToGrpc(Abstraction.Models.Message.MessageModel input)
            => new GrpcModel_Message
            {
                Id = input.Id,
                Serial = input.Serial,
                CreationTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(input.CreationTime),
                Status = (Abstraction.GrpcInterface.GrpcMessageStatus)input.Status,
                Priority = (Abstraction.GrpcInterface.GrpcMailPriority)input.Priority,
                SubjectIsHtml = input.SubjectIsHtml,
                BodyIsHtml = input.BodyIsHtml,
                Subject = input.Subject,
                Body = input.Body,
                Bcc = { (input.Bcc ?? []).Select(ToGrpc) },
                Cc = { (input.Cc ?? []).Select(ToGrpc) },
                To = { (input.To ?? []).Select(ToGrpc) },
                ReplyTo = { (input.ReplyTo ?? []).Select(ToGrpc) },
                From = input.From is null ? null : ToGrpc(input.From),
                Headers = { input.Headers },
            };

        public GrpcResult_MessageModel ToGrpc(Abstraction.Models.Result<Abstraction.Models.Message.MessageModel> input)
            => new GrpcResult_MessageModel
            {
                IsSuccessful = input.IsSuccessful,
                Errors = { ModelMapper.Instance.ToGrpcReasonArray(input.Errors) },
                Warnings = { ModelMapper.Instance.ToGrpcReasonArray(input.Warnings) },
                Data = input.IsSuccessful ? ToGrpc(input.Data!) : null,
            };

        public GrpcListResult_MessageModel ToGrpc(Abstraction.Models.ListResult<Abstraction.Models.Message.MessageModel> input)
            => new GrpcListResult_MessageModel
            {
                IsSuccessful = input.IsSuccessful,
                Errors = { ModelMapper.Instance.ToGrpcReasonArray(input.Errors) },
                Warnings = { ModelMapper.Instance.ToGrpcReasonArray(input.Warnings) },
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalItems = input.TotalItems,
                Data = { input.IsSuccessful ? input.Data.Select(ToGrpc) : [] }
            };
    }
}
