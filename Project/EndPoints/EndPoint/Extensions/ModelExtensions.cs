namespace Dariosoft.EmailSender.EndPoint
{

    public static class ModelExtensions
    {
        #region ClientModel
        public static Core.Models.CreateClientModel Patch(this Abstraction.Models.Client.CreateClientModel model)
            => new Core.Models.CreateClientModel
            {
                Enabled = model.Enabled,
                Name = model.Name,
                Description = model.Description,
                AdminUserName = model.AdminUserName,
                AdminPassword = model.AdminPassword,
            };

        public static Core.Models.UpdateClientModel Patch(this Abstraction.Models.Client.UpdateClientModel model)
        {
            return new Core.Models.UpdateClientModel
            {
                Key = model.Key!,
                Enabled = model.Enabled,
                Name = model.Name,
                Description = model.Description,
                AdminUserName = model.AdminUserName,
            };
        }

        public static Abstraction.Models.Client.ClientModel Unpatch(this Core.Models.ClientModel model)
            => new Abstraction.Models.Client.ClientModel
            {
                Id = model.Id.ToString().ToLower(),
                Serial = model.Serial,
                CreationTime = model.CreationTime,
                Enabled = model.Enabled,
                Name = model.Name,
                Description = model.Description,
                AccessKey = model.ApiKey,
                AdminUserName = model.AdminUserName,
            };
        #endregion

        #region HostModel
        public static Core.Models.CreateHostModel Patch(this Abstraction.Models.Host.CreateHostModel model)
            => new Core.Models.CreateHostModel
            {

                ClientKey = model.ClientKey,
                Enabled = model.Enabled,
                Address = model.Address,
                PortNumber = model.PortNumber,
                UseSsl = model.UseSsl,
                Description = model.Description,
            };

        public static Core.Models.UpdateHostModel Patch(this Abstraction.Models.Host.UpdateHostModel model)
        {
            return new Core.Models.UpdateHostModel
            {
                Key = model.Key!,
                ClientKey = model.ClientKey,
                Enabled = model.Enabled,
                Address = model.Address,
                PortNumber = model.PortNumber,
                UseSsl = model.UseSsl,
                Description = model.Description,
            };
        }

        public static Abstraction.Models.Host.HostModel Unpatch(this Core.Models.HostModel model)
            => new Abstraction.Models.Host.HostModel
            {
                Id = model.Id.ToString().ToLower(),
                Serial = model.Serial,
                CreationTime = model.CreationTime,
                ClientId = model.ClientId,
                ClientName = model.ClientName,
                Enabled = model.Enabled,
                Address = model.Address,
                PortNumber = model.PortNumber,
                UseSsl = model.UseSsl,
                Description = model.Description,
            };
        #endregion

        #region AccountModel
        public static Core.Models.CreateAccountModel Patch(this Abstraction.Models.Account.CreateAccountModel model)
            => new Core.Models.CreateAccountModel
            {
                ClientKey = model.ClientKey!,
                HostKey = model.HostKey!,
                EmailAddress = model.EmailAddress,
                DisplayName = model.DisplayName,
                Password = model.Password,
                Enabled = model.Enabled,
                Description = model.Description,
            };

        public static Core.Models.UpdateAccountModel Patch(this Abstraction.Models.Account.UpdateAccountModel model)
        {
            return new Core.Models.UpdateAccountModel
            {
                Key = model.Key!,
                ClientKey = model.ClientKey!,
                HostKey = model.HostKey!,
                EmailAddress = model.EmailAddress,
                DisplayName = model.DisplayName,
                Password = model.Password,
                Enabled = model.Enabled,
                Description = model.Description,
            };
        }

        public static Abstraction.Models.Account.AccountModel Unpatch(this Core.Models.AccountModel model)
            => new Abstraction.Models.Account.AccountModel
            {
                Id = model.Id.ToString().ToLower(),
                Serial = model.Serial,
                CreationTime = model.CreationTime,
                ClientId = model.ClientId.ToString().ToLower(),
                ClientName = model.ClientName,
                HostId = model.HostId.ToString().ToLower(),
                Host = model.Host,
                Enabled = model.Enabled,
                EmailAddress = model.EmailAddress,
                DisplayName = model.DisplayName,
                Password = model.Password,
                Description = model.Description,
            };
        #endregion

        #region MessageModel
        public static Core.Models.CreateMessageModel Patch(this Abstraction.Models.Message.CreateMessageModel model)
            => new Core.Models.CreateMessageModel
            {
                SourceAccountKey = model.SourceAccountKey!,
                IsDraft = model.IsDraft,
                From = model.From?.Patch(),
                Subject = model.Subject,
                SubjectIsHtml = model.SubjectIsHtml,
                Body = model.Body,
                BodyIsHtml = model.BodyIsHtml,
                Priority = model.Priority,
                To = model.To.Select(e => e.Patch()).ToArray(),
                Cc = model.Cc?.Select(e => e.Patch()).ToArray(),
                Bcc = model.Bcc?.Select(e => e.Patch()).ToArray(),
                ReplyTo = model.ReplyTo?.Select(e => e.Patch()).ToArray(),
                Headers = model.Headers,
            };

        public static Core.Models.UpdateMessageModel Patch(this Abstraction.Models.Message.UpdateMessageModel model)
        {
            return new Core.Models.UpdateMessageModel
            {
                Key = model.Key!,
                SourceAccountKey = model.SourceAccountKey!,
                From = model.From?.Patch(),
                Subject = model.Subject,
                SubjectIsHtml = model.SubjectIsHtml,
                Body = model.Body,
                BodyIsHtml = model.BodyIsHtml,
                Priority = model.Priority,
                To = model.To.Select(e => e.Patch()).ToArray(),
                Cc = model.Cc?.Select(e => e.Patch()).ToArray(),
                Bcc = model.Bcc?.Select(e => e.Patch()).ToArray(),
                ReplyTo = model.ReplyTo?.Select(e => e.Patch()).ToArray(),
                Headers = model.Headers,
            };
        }

        public static Abstraction.Models.Message.MessageModel Unpatch(this Core.Models.MessageModel model)
            => new Abstraction.Models.Message.MessageModel
            {
                Id = model.Id.ToString().ToLower(),
                Serial = model.Serial,
                CreationTime = model.CreationTime,
                Status = model.Status,
                SubjectIsHtml = model.SubjectIsHtml,
                Subject = model.Subject,
                Body = model.Body,
                BodyIsHtml = model.BodyIsHtml,
                Priority = model.Priority,
                NumberOfTries = model.NumberOfTries,
                From = model.From?.Unpatch(),
                To = model.To.Select(e => e.Unpatch()).ToArray(),
                Cc = model.Cc?.Select(e => e.Unpatch()).ToArray(),
                Bcc = model.Bcc?.Select(e => e.Unpatch()).ToArray(),
                ReplyTo = model.ReplyTo?.Select(e => e.Unpatch()).ToArray(),
                Headers = model.Headers,
            };
        #endregion

        #region Misc
        public static System.Net.Mail.MailAddress Patch(this Abstraction.Models.Common.MailAddress mailAddress)
            => new System.Net.Mail.MailAddress(mailAddress.Address, mailAddress.DisplayName);

        public static Abstraction.Models.Common.MailAddress Unpatch(this System.Net.Mail.MailAddress mailAddress)
            => new Abstraction.Models.Common.MailAddress { Address = mailAddress.Address, DisplayName = mailAddress.DisplayName };

        public static Framework.Types.ListQueryModel Patch(this Abstraction.Models.Common.ListQueryModel model)
            => new Framework.Types.ListQueryModel
            {
                Page = model.Page,
                PageSize = model.PageSize,
                Query = model.Query,
                DescendingSort = model.DescendingSort,
                SortBy = model.SortBy,
                Parameters = model.Parameters
            };

        public static Core.Models.SetAvailabilityModel Patch(this Abstraction.Models.Common.SetAvailabilityModel model)
        {
            Core.Models.KeyModel key = model.Key;

            return new Core.Models.SetAvailabilityModel
            {
                Id = key.Id,
                Serial = key.Serial,
                Enabled = model.Enabled
            };
        }

        public static Abstraction.Models.Common.BaseModel Unpatch(this Core.Models.BaseModel model)
          => new Abstraction.Models.Common.BaseModel
          {
              Id = model.Id.ToString(),
              Serial = model.Serial,
              CreationTime = model.CreationTime
          };
        #endregion
    }
}
