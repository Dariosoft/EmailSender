using Dariosoft.EmailSender.Infrastructure.Database.DataSource.Tables;
using FluentMigrator;
using System.Net;

namespace Dariosoft.EmailSender.Infrastructure.Database.Migrations
{
    [Migration(2024_03_21_1515_02, description: "Create tables")]
    public class M2024_03_21_15_15_02 : DariosoftMigration
    {
        public override void Up()
        {
            const string superAdmin_Title = "Super Admin",
                superAdmin_UserName = "sa-email-sender",
                superAdmin_Password = "SA20032024";

            string tblAdmin = nameof(DataSource.Tables.Admin)
                , tblClient = nameof(DataSource.Tables.Client)
                , tblHost = nameof(DataSource.Tables.Host)
                , tblAccount = nameof(DataSource.Tables.Account)
                , tblMailAddressCollection = nameof(DataSource.Tables.MailAddressCollection)
                , tblMailAddressCollectionItem = nameof(DataSource.Tables.MailAddressCollectionItem)
                , tblMessage = nameof(DataSource.Tables.Message)
                , tblMessageTrySendLog = nameof(DataSource.Tables.MessageTrySendLog);

            #region 1. Create Table: core.Admin
            CreateFlaggedTable(DataSource.DbSchema.Core, tblAdmin)
                .WithColumn(nameof(DataSource.Tables.Admin.Title)).AsString(64)
                .WithColumn(nameof(DataSource.Tables.Admin.TitleRAW)).AsString(64)
                .WithColumn(nameof(DataSource.Tables.Admin.UserName)).AsString(64).Unique("UQ_core_Admin_UserName")
                .WithColumn(nameof(DataSource.Tables.Admin.Password)).AsString(255)
                .WithColumn(nameof(DataSource.Tables.Admin.IsSuperAdmin)).AsBoolean();

            SetSerialIdentity(DataSource.DbSchema.Core, tblAdmin);

            Insert.IntoTable(tblAdmin)
                .InSchema(DataSource.DbSchema.Core)
                .Row(new {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.UtcNow,
                    Flags = 0,
                    IsSuperAdmin = true,
                    Title = superAdmin_Title.Clean()!,
                    TitleRAW = superAdmin_Title,
                    UserName = superAdmin_UserName,
                    Password = Framework.Cryptography.PasswordEncoder.Encode(superAdmin_UserName, superAdmin_Password)
                });
            #endregion

            #region 2. Create Table: core.Client
            CreateFlaggedTable(DataSource.DbSchema.Core, tblClient)
                .WithColumn(nameof(DataSource.Tables.Client.Name)).AsString(128)
                .WithColumn(nameof(DataSource.Tables.Client.NameRAW)).AsString(128)
                .WithColumn(nameof(DataSource.Tables.Client.ApiKey)).AsString(255).Nullable()
                .WithColumn(nameof(DataSource.Tables.Client.Description)).AsString(255).Nullable()
                .WithColumn(nameof(DataSource.Tables.Client.DescriptionRAW)).AsString(255).Nullable();

            SetSerialIdentity(DataSource.DbSchema.Core, tblClient);
            #endregion

            #region 3. Create Table: core.Host
            CreateFlaggedTable(DataSource.DbSchema.Core, tblHost)
                .WithColumn(nameof(DataSource.Tables.Host.ClientId)).AsGuid().Nullable()
                .WithColumn(nameof(DataSource.Tables.Host.Address)).AsString(255)
                .WithColumn(nameof(DataSource.Tables.Host.PortNumber)).AsInt32()
                .WithColumn(nameof(DataSource.Tables.Host.UseSsl)).AsBoolean()
                .WithColumn(nameof(DataSource.Tables.Host.Description)).AsString(255).Nullable()
                .WithColumn(nameof(DataSource.Tables.Host.DescriptionRAW)).AsString(255).Nullable();

            Create.ForeignKey("FK_core_Host_Client")
                .FromTable(tblHost)
                .InSchema(DataSource.DbSchema.Core)
                .ForeignColumn(nameof(DataSource.Tables.Host.ClientId))
                .ToTable(tblClient)
                .InSchema(DataSource.DbSchema.Core)
                .PrimaryColumn(nameof(DataSource.Tables.FlaggedTable.Id));

            SetSerialIdentity(DataSource.DbSchema.Core, tblHost);

            Insert.IntoTable(tblHost)
                .InSchema(DataSource.DbSchema.Core)
                .Row(new { 
                    Id = Guid.NewGuid(),
                    Flags = 0,
                    CreationTime = DateTime.UtcNow,
                    ClientId = (Guid?)null,
                    Address = "smtp-relay.gmail.com",
                    PortNumber = 587,
                    UseSsl = true,
                    Description = "google smtp relay",
                    DescriptionRAW = "Google SMTP Relay"
                });
            #endregion

            #region 4. Create Table: core.Account
            CreateFlaggedTable(DataSource.DbSchema.Core, tblAccount)
                .WithColumn(nameof(DataSource.Tables.Account.ClientId)).AsGuid()
                .WithColumn(nameof(DataSource.Tables.Account.HostId)).AsGuid()
                .WithColumn(nameof(DataSource.Tables.Account.EmailAddress)).AsString(128)
                .WithColumn(nameof(DataSource.Tables.Account.Password)).AsString(255)
                .WithColumn(nameof(DataSource.Tables.Account.DisplayName)).AsString(255).Nullable()
                .WithColumn(nameof(DataSource.Tables.Account.DisplayNameRAW)).AsString(255).Nullable()
                .WithColumn(nameof(DataSource.Tables.Account.Description)).AsString(255).Nullable()
                .WithColumn(nameof(DataSource.Tables.Account.DescriptionRAW)).AsString(255).Nullable();

            Create.ForeignKey("FK_core_Account_Client")
                .FromTable(tblAccount)
                .InSchema(DataSource.DbSchema.Core)
                .ForeignColumn(nameof(DataSource.Tables.Account.ClientId))
                .ToTable(tblClient)
                .InSchema(DataSource.DbSchema.Core)
                .PrimaryColumn(nameof(DataSource.Tables.FlaggedTable.Id));

            Create.ForeignKey("FK_core_Account_Host")
                .FromTable(tblAccount)
                .InSchema(DataSource.DbSchema.Core)
                .ForeignColumn(nameof(DataSource.Tables.Account.HostId))
                .ToTable(tblHost)
                .InSchema(DataSource.DbSchema.Core)
                .PrimaryColumn(nameof(DataSource.Tables.FlaggedTable.Id));

            SetSerialIdentity(DataSource.DbSchema.Core, tblAccount);
            #endregion

            #region 5. Create Table: core.MailAddressCollection
            CreateFlaggedTable(DataSource.DbSchema.Core, tblMailAddressCollection)
                .WithColumn(nameof(DataSource.Tables.MailAddressCollection.ClientId)).AsGuid()
                .WithColumn(nameof(DataSource.Tables.MailAddressCollection.Name)).AsString(255)
                .WithColumn(nameof(DataSource.Tables.MailAddressCollection.NameRAW)).AsString(255)
                .WithColumn(nameof(DataSource.Tables.MailAddressCollection.Description)).AsString(255).Nullable()
                .WithColumn(nameof(DataSource.Tables.MailAddressCollection.DescriptionRAW)).AsString(255).Nullable();

            Create.ForeignKey("FK_core_MailAddressCollection_Client")
                .FromTable(tblMailAddressCollection)
                .InSchema(DataSource.DbSchema.Core)
                .ForeignColumn(nameof(DataSource.Tables.MailAddressCollection.ClientId))
                .ToTable(tblClient)
                .InSchema(DataSource.DbSchema.Core)
                .PrimaryColumn(nameof(DataSource.Tables.FlaggedTable.Id));

            SetSerialIdentity(DataSource.DbSchema.Core, tblMailAddressCollection);
            #endregion

            #region 6. Create Table: core.MailAddressCollectionItem
            CreateTable(DataSource.DbSchema.Core, tblMailAddressCollectionItem)
                .WithColumn(nameof(DataSource.Tables.MailAddressCollectionItem.CollectionId)).AsGuid()
                .WithColumn(nameof(DataSource.Tables.MailAddressCollectionItem.Address)).AsString(255)
                .WithColumn(nameof(DataSource.Tables.MailAddressCollectionItem.DisplayName)).AsString(255).Nullable()
                .WithColumn(nameof(DataSource.Tables.MailAddressCollectionItem.DisplayNameRAW)).AsString(255).Nullable();

            Create.ForeignKey("FK_core_MailAddressCollectionItem_MailAddressCollection")
                .FromTable(tblMailAddressCollectionItem)
                .InSchema(DataSource.DbSchema.Core)
                .ForeignColumn(nameof(DataSource.Tables.MailAddressCollectionItem.CollectionId))
                .ToTable(tblMailAddressCollection)
                .InSchema(DataSource.DbSchema.Core)
                .PrimaryColumn(nameof(DataSource.Tables.FlaggedTable.Id));

            SetSerialIdentity(DataSource.DbSchema.Core, tblMailAddressCollectionItem);
            #endregion

            #region 7. Create Table: core.Message
            CreateFlaggedTable(DataSource.DbSchema.Core, tblMessage)
                .WithColumn(nameof(DataSource.Tables.Message.AccountId)).AsGuid()
                .WithColumn(nameof(DataSource.Tables.Message.Subject)).AsString(512)
                .WithColumn(nameof(DataSource.Tables.Message.SubjectRAW)).AsString(512)
                .WithColumn(nameof(DataSource.Tables.Message.SubjectIsHtml)).AsBoolean()
                .WithColumn(nameof(DataSource.Tables.Message.Body)).AsString()
                .WithColumn(nameof(DataSource.Tables.Message.BodyIsHtml)).AsBoolean()
                .WithColumn(nameof(DataSource.Tables.Message.Priority)).AsInt16()
                .WithColumn(nameof(DataSource.Tables.Message.Status)).AsInt16()
                .WithColumn(nameof(DataSource.Tables.Message.LastStatusTime)).AsDateTime()
                .WithColumn(nameof(DataSource.Tables.Message.NumberOfTries)).AsInt16()
                .WithColumn(nameof(DataSource.Tables.Message.From)).AsString(1024).Nullable()
                .WithColumn(nameof(DataSource.Tables.Message.To)).AsGuid()
                .WithColumn(nameof(DataSource.Tables.Message.Cc)).AsGuid().Nullable()
                .WithColumn(nameof(DataSource.Tables.Message.Bcc)).AsGuid().Nullable()
                .WithColumn(nameof(DataSource.Tables.Message.ReplyTo)).AsGuid().Nullable()
                .WithColumn(nameof(DataSource.Tables.Message.Headers)).AsString().Nullable();

            Create.ForeignKey("FK_core_Message_Account")
                .FromTable(tblMessage)
                .InSchema(DataSource.DbSchema.Core)
                .ForeignColumn(nameof(DataSource.Tables.Message.AccountId))
                .ToTable(tblAccount)
                .InSchema(DataSource.DbSchema.Core)
                .PrimaryColumn(nameof(DataSource.Tables.FlaggedTable.Id));

            SetSerialIdentity(DataSource.DbSchema.Core, tblMessage);
            #endregion

            #region 8. Create Table: core.MessageTrySendLog
            CreateTable(DataSource.DbSchema.Core, tblMessageTrySendLog)
                .WithColumn(nameof(DataSource.Tables.MessageTrySendLog.MessageId)).AsGuid()
                .WithColumn(nameof(DataSource.Tables.MessageTrySendLog.When)).AsDateTime()
                .WithColumn(nameof(DataSource.Tables.MessageTrySendLog.Status)).AsInt16()
                .WithColumn(nameof(DataSource.Tables.MessageTrySendLog.Description)).AsString();

            Create.ForeignKey("FK_core_MessageTrySendLog_Message")
                .FromTable(tblMessageTrySendLog)
                .InSchema(DataSource.DbSchema.Core)
                .ForeignColumn(nameof(DataSource.Tables.MessageTrySendLog.MessageId))
                .ToTable(tblMessage)
                .InSchema(DataSource.DbSchema.Core)
                .PrimaryColumn(nameof(DataSource.Tables.FlaggedTable.Id));

            SetSerialIdentity(DataSource.DbSchema.Core, tblMessageTrySendLog);
            #endregion
        }

        public override void Down()
        {
            Delete.Table(nameof(DataSource.Tables.MessageTrySendLog)).InSchema(DataSource.DbSchema.Core);
            Delete.Table(nameof(DataSource.Tables.Message)).InSchema(DataSource.DbSchema.Core);
            Delete.Table(nameof(DataSource.Tables.MailAddressCollectionItem)).InSchema(DataSource.DbSchema.Core);
            Delete.Table(nameof(DataSource.Tables.MailAddressCollection)).InSchema(DataSource.DbSchema.Core);
            Delete.Table(nameof(DataSource.Tables.Account)).InSchema(DataSource.DbSchema.Core);
            Delete.Table(nameof(DataSource.Tables.Host)).InSchema(DataSource.DbSchema.Core);
            Delete.Table(nameof(DataSource.Tables.Client)).InSchema(DataSource.DbSchema.Core);
            Delete.Table(nameof(DataSource.Tables.Admin)).InSchema(DataSource.DbSchema.Core);
        }
    }
}
