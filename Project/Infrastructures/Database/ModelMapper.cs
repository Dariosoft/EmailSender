using System.Net.Mail;
using System.Reflection;

namespace Dariosoft.EmailSender.Infrastructure.Database
{
    internal static class ModelMapper
    {
        #region Client
        public static DataSource.Tables.Admin? ToAdminEntity(Core.Models.ClientModel? model)
        {
            return model is null
                ? null
                : new DataSource.Tables.Admin
                {
                    Id = model.Id,
                    Serial = 0,
                    CreationTime = model.CreationTime.UtcDateTime,
                    Flags = model.Enabled ? DataSource.RecordFlag.None : DataSource.RecordFlag.Disable,
                    IsSuperAdmin = false,
                    Title = model.Name.Clean() ?? "",
                    TitleRAW = model.Name,
                    UserName = model.AdminUserName.Trim().ToLower(),
                    Password = model.AdminPassword
                };
        }

        public static DataSource.Tables.Client? ToEntity(Core.Models.ClientModel? model)
        {
            return model is null
                ? null
                : new DataSource.Tables.Client
                {
                    Id = model.Id,
                    Serial = model.Serial,
                    CreationTime = model.CreationTime.UtcDateTime,
                    Flags = model.Enabled ? DataSource.RecordFlag.None : DataSource.RecordFlag.Disable,
                    ApiKey = model.ApiKey,
                    Name = model.Name.Clean() ?? "",
                    NameRAW = model.Name,
                    Description = model.Description.Clean(),
                    DescriptionRAW = model.Description
                };
        }

        public static Core.Models.ClientModel? ToModel(DataSource.Tables.Client? entity, DataSource.Tables.Admin? admin)
        {
            return entity is null
                ? null
                : new Core.Models.ClientModel
                {
                    Id = entity.Id,
                    Serial = entity.Serial,
                    Enabled = !entity.Flags.HasFlag(DataSource.RecordFlag.Disable),
                    CreationTime = entity.CreationTime,
                    AdminUserName = admin?.UserName ?? "",
                    AdminPassword = admin?.Password ?? "",
                    ApiKey = entity.ApiKey ?? "",
                    Name = entity.NameRAW,
                    Description = entity.DescriptionRAW,
                };
        }
        #endregion

        #region Host
        public static DataSource.Tables.Host? ToEntity(Core.Models.HostModel? model)
        {
            return model is null
                ? null
                : new DataSource.Tables.Host
                {
                    Id = model.Id,
                    ClientId = model.ClientId,
                    Serial = model.Serial,
                    CreationTime = model.CreationTime.UtcDateTime,
                    Flags = model.Enabled ? DataSource.RecordFlag.None : DataSource.RecordFlag.Disable,
                    PortNumber = model.PortNumber,
                    UseSsl = model.UseSsl,
                    Address = model.Address,
                    Description = model.Description.Clean(),
                    DescriptionRAW = model.Description
                };
        }

        public static Core.Models.HostModel? ToModel(DataSource.Tables.Host? entity)
        {
            return entity is null
                ? null
                : new Core.Models.HostModel
                {
                    Id = entity.Id,
                    ClientId = entity.ClientId,
                    Serial = entity.Serial,
                    Enabled = !entity.Flags.HasFlag(DataSource.RecordFlag.Disable),
                    CreationTime = entity.CreationTime,
                    UseSsl = entity.UseSsl,
                    Address = entity.Address,
                    PortNumber = entity.PortNumber,
                    Description = entity.DescriptionRAW,
                };
        }
        #endregion

        #region Account
        public static DataSource.Tables.Account? ToEntity(Core.Models.AccountModel? model)
        {
            return model is null
                ? null
                : new DataSource.Tables.Account
                {
                    Id = model.Id,
                    ClientId = model.ClientId,
                    HostId = model.HostId,
                    Serial = model.Serial,
                    CreationTime = model.CreationTime.UtcDateTime,
                    Flags = model.Enabled ? DataSource.RecordFlag.None : DataSource.RecordFlag.Disable,
                    EmailAddress = model.EmailAddress.Trim().ToLower(),
                    Password = model.Password,
                    DisplayName = model.DisplayName.Clean(),
                    DisplayNameRAW = model.DisplayName,
                    Description = model.Description.Clean(),
                    DescriptionRAW = model.Description
                };
        }

        public static Core.Models.AccountModel? ToModel(DataSource.Tables.Account? entity)
        {
            return entity is null
                ? null
                : new Core.Models.AccountModel
                {
                    Id = entity.Id,
                    ClientId = entity.ClientId,
                    HostId = entity.HostId,
                    Serial = entity.Serial,
                    Enabled = !entity.Flags.HasFlag(DataSource.RecordFlag.Disable),
                    CreationTime = entity.CreationTime,
                    EmailAddress = entity.EmailAddress,
                    Password = entity.Password,
                    DisplayName = entity.DisplayNameRAW,
                    Description = entity.DescriptionRAW,
                };
        }
        #endregion

        #region MailAddress
        public static MailAddress? ParseToMailAddress(string? address)
        {
            if (string.IsNullOrWhiteSpace(address) || !address.IsValidEmailAddress())
                return null;

            var i = address.IndexOf(';');

            return MailAddress.TryCreate(address: i > 0 ? address[..(i - 1)] : address, displayName: i > 0 ? address[(i + 1)..] : null, out var mailAddress)
                ? mailAddress
                : null;
        }
        #endregion

        #region Message
        public static DataSource.Tables.Message? ToEntity(Core.Models.MessageModel? model, List<DataSource.Tables.MailAddressCollection> mailAddressCollections, List<DataSource.Tables.MailAddressCollectionItem> mailAddressCollectionItems)
        {
            if (model is null) return null;

            model.Bcc = model.Bcc?.Where(e => e is not null)?.ToArray();
            model.Cc = model.Cc?.Where(e => e is not null)?.ToArray();
            model.ReplyTo = model.ReplyTo?.Where(e => e is not null)?.ToArray();

            var entity = new DataSource.Tables.Message
            {
                Id = model.Id,
                AccountId = model.AccountId,
                Serial = model.Serial,
                Flags = DataSource.RecordFlag.None,
                CreationTime = model.CreationTime.UtcDateTime,
                LastStatusTime = model.LastStatusTime.UtcDateTime,
                Priority = model.Priority,
                Status = model.Status,
                SubjectIsHtml = model.SubjectIsHtml,
                Subject = model.Subject.Clean() ?? "",
                SubjectRAW = model.Subject,
                BodyIsHtml = model.BodyIsHtml,
                Body = model.Body,
                From = model.From?.ToString(),
                To = Guid.NewGuid(),
                Bcc = model.Bcc is null || model.Bcc.Length == 0 ? null : Guid.NewGuid(),
                Cc = model.Cc is null || model.Cc.Length == 0 ? null : Guid.NewGuid(),
                ReplyTo = model.ReplyTo is null || model.ReplyTo.Length == 0 ? null : Guid.NewGuid(),
                Headers = JSON.CamelCase.Serialize(model.Headers),
            };

            mailAddressCollections.Add(new DataSource.Tables.MailAddressCollection
            {
                Id = entity.To,
                ClientId = Guid.Empty,
                CreationTime = entity.CreationTime,
                Flags = DataSource.RecordFlag.None,
                Name = entity.Id.ToString().ToLower(),
                NameRAW = entity.Id.ToString().ToLower(),
                Description = "MailMessage.To".Clean(exceptions: "."),
                DescriptionRAW = "MailMessage.To"
            });

            mailAddressCollectionItems.AddRange(model.To.Select(e => new DataSource.Tables.MailAddressCollectionItem
            {
                Id = Guid.NewGuid(),
                Serial = 0,
                CollectionId = entity.To,
                Address = e.Address,
                DisplayName = e.DisplayName.Clean(),
                DisplayNameRAW = e.DisplayName,
            }));

            if (entity.Bcc != null && entity.Bcc != Guid.Empty)
            {
                mailAddressCollections.Add(new DataSource.Tables.MailAddressCollection
                {
                    Id = entity.Bcc.Value,
                    ClientId = Guid.Empty,
                    CreationTime = entity.CreationTime,
                    Flags = DataSource.RecordFlag.None,
                    Name = entity.Id.ToString().ToLower(),
                    NameRAW = entity.Id.ToString().ToLower(),
                    Description = "MailMessage.Bcc".Clean(exceptions: "."),
                    DescriptionRAW = "MailMessage.Bcc"
                });

                mailAddressCollectionItems.AddRange(model.Bcc!
                    .Select(e => new DataSource.Tables.MailAddressCollectionItem
                    {
                        Id = Guid.NewGuid(),
                        Serial = 0,
                        CollectionId = entity.Bcc.Value,
                        Address = e.Address,
                        DisplayName = e.DisplayName.Clean(),
                        DisplayNameRAW = e.DisplayName,
                    }));
            }

            if (entity.Cc != null && entity.Cc != Guid.Empty)
            {
                mailAddressCollections.Add(new DataSource.Tables.MailAddressCollection
                {
                    Id = entity.Cc.Value,
                    ClientId = Guid.Empty,
                    CreationTime = entity.CreationTime,
                    Flags = DataSource.RecordFlag.None,
                    Name = entity.Id.ToString().ToLower(),
                    NameRAW = entity.Id.ToString().ToLower(),
                    Description = "MailMessage.Cc".Clean(exceptions: "."),
                    DescriptionRAW = "MailMessage.Cc"
                });

                mailAddressCollectionItems.AddRange(model.Cc!
                    .Select(e => new DataSource.Tables.MailAddressCollectionItem
                    {
                        Id = Guid.NewGuid(),
                        Serial = 0,
                        CollectionId = entity.Cc.Value,
                        Address = e.Address,
                        DisplayName = e.DisplayName.Clean(),
                        DisplayNameRAW = e.DisplayName,
                    }));
            }

            if (entity.ReplyTo != null && entity.ReplyTo != Guid.Empty)
            {
                mailAddressCollections.Add(new DataSource.Tables.MailAddressCollection
                {
                    Id = entity.ReplyTo.Value,
                    ClientId = Guid.Empty,
                    CreationTime = entity.CreationTime,
                    Flags = DataSource.RecordFlag.None,
                    Name = entity.Id.ToString().ToLower(),
                    NameRAW = entity.Id.ToString().ToLower(),
                    Description = "MailMessage.ReplyTo".Clean(exceptions: "."),
                    DescriptionRAW = "MailMessage.ReplyTo"
                });

                mailAddressCollectionItems.AddRange(model.ReplyTo!
                    .Select(e => new DataSource.Tables.MailAddressCollectionItem
                    {
                        Id = Guid.NewGuid(),
                        Serial = 0,
                        CollectionId = entity.ReplyTo.Value,
                        Address = e.Address,
                        DisplayName = e.DisplayName.Clean(),
                        DisplayNameRAW = e.DisplayName,
                    }));
            }

            return entity;
        }

        public static Core.Models.MessageModel? ToModel(
            DataSource.Tables.Message? entity
            , MailAddress[] to
            , MailAddress[]? bcc = null
            , MailAddress[]? cc = null
            , MailAddress[]? replyTo = null)
        {
            return entity is null
                ? null
                : new Core.Models.MessageModel
                {
                    Id = entity.Id,
                    AccountId = entity.AccountId,
                    Serial = entity.Serial,
                    CreationTime = entity.CreationTime,
                    LastStatusTime = entity.LastStatusTime,
                    Priority = entity.Priority,
                    Status = entity.Status,
                    SubjectIsHtml = entity.SubjectIsHtml,
                    Subject = entity.SubjectRAW,
                    BodyIsHtml = entity.BodyIsHtml,
                    Body = entity.Body,
                    From = ParseToMailAddress(entity.From),
                    To = to,
                    Bcc = bcc?.Length == 0 ? null : bcc,
                    Cc = cc?.Length == 0 ? null : cc,
                    ReplyTo = replyTo?.Length == 0 ? null : replyTo,
                    Headers = JSON.CamelCase.Deserialize<IDictionary<string, string>>(entity.Headers)
                };
        }
        #endregion
    }
}
