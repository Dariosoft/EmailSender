using LinqToDB.Data;
using System.Net.Mail;

namespace Dariosoft.EmailSender.Infrastructure.Database.Repositories
{
    internal class MessageRepository(RepositoryInjection injection) : Repository(injection), Core.Repositories.IMessageRepository
    {
        public async Task<Reply> Create(Request<Core.Models.MessageModel> request)
        {
            var mailAddressCollections = new List<DataSource.Tables.MailAddressCollection>();
            var mailAddressCollectionItems = new List<DataSource.Tables.MailAddressCollectionItem>();

            try
            {
                var entity = ModelMapper.ToEntity(request.Payload, mailAddressCollections, mailAddressCollectionItems)!;

                await using (var context = GetDbContext())
                {
                    var ClientId = await context.Accounts
                        .Where(e => e.Id == entity.AccountId)
                        .Select(e => e.ClientId)
                        .FirstOrDefaultAsync();

                    for (var i = 0; i < mailAddressCollections.Count; i++)
                        mailAddressCollections[i].ClientId = ClientId;

                    using (var transaction = await context.BeginTransactionAsync())
                    {
                        try
                        {
                            await context.MailAddressCollections.BulkCopyAsync(mailAddressCollections);
                            await context.MailAddressCollectionItems.BulkCopyAsync(mailAddressCollectionItems);

                            await context.InsertAsync(entity, schemaName: DataSource.DbSchema.Core);

                            await transaction.CommitAsync();
                        }
                        catch
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }

                        request.Payload.Serial = entity.Serial = await context.Messages.Where(e => e.Id == entity.Id)
                            .Select(e => e.Serial)
                            .FirstOrDefaultAsync();
                    }
                }

                return Reply.Success();
            }
            catch (Exception e)
            {
                return Fail(request, nameof(Create), e);
            }
            finally
            {
                mailAddressCollectionItems.Clear();
                mailAddressCollections.Clear();
            }
        }

        public async Task<Reply> Update(Request<Core.Models.MessageModel> request)
        {
            var mailAddressCollections = new List<DataSource.Tables.MailAddressCollection>();
            var mailAddressCollectionItems = new List<DataSource.Tables.MailAddressCollectionItem>();

            try
            {
                var entity = ModelMapper.ToEntity(request.Payload, mailAddressCollections, mailAddressCollectionItems)!;

                await using (var context = GetDbContext())
                {
                    var existance = await context.Messages.FirstOrDefaultAsync(e => e.Id == entity.Id);

                    if (existance is not null)
                    {
                        for (int i = 0; i < mailAddressCollectionItems.Count; i++)
                        {
                            if (mailAddressCollectionItems[i].CollectionId == entity.To)
                                mailAddressCollectionItems[i].CollectionId = existance.To;

                            if (entity.Cc != null && existance.Cc != null && mailAddressCollectionItems[i].CollectionId == entity.Cc)
                                mailAddressCollectionItems[i].CollectionId = existance.Cc.Value;

                            if (entity.Bcc != null && existance.Bcc != null && mailAddressCollectionItems[i].CollectionId == entity.Bcc)
                                mailAddressCollectionItems[i].CollectionId = existance.Bcc.Value;

                            if (entity.ReplyTo != null && existance.ReplyTo != null && mailAddressCollectionItems[i].CollectionId == entity.ReplyTo)
                                mailAddressCollectionItems[i].CollectionId = existance.ReplyTo.Value;
                        }

                        for (int i = 0; i < mailAddressCollections.Count; i++)
                        {
                            if (mailAddressCollections[i].Id == entity.To)
                                mailAddressCollections[i].Id = existance.To;

                            if (entity.Cc != null && existance.Cc != null && mailAddressCollections[i].Id == entity.Cc)
                                mailAddressCollections[i].Id = existance.Cc.Value;

                            if (entity.Bcc != null && existance.Bcc != null && mailAddressCollections[i].Id == entity.Bcc)
                                mailAddressCollections[i].Id = existance.Bcc.Value;

                            if (entity.ReplyTo != null && existance.ReplyTo != null && mailAddressCollections[i].Id == entity.ReplyTo)
                                mailAddressCollections[i].Id = existance.ReplyTo.Value;
                        }

                        entity.To = existance.To;
                        if (entity.Cc != null && existance.Cc != null) entity.Cc = existance.Cc.Value;
                        if (entity.Bcc != null && existance.Bcc != null) entity.Bcc = existance.Bcc.Value;
                        if (entity.ReplyTo != null && existance.ReplyTo != null) entity.ReplyTo = existance.ReplyTo.Value;

                        var removes = mailAddressCollections.RemoveAll(e => e.Id == entity.To || e.Id == entity.Cc || e.Id == entity.Bcc || e.Id == entity.ReplyTo);

                        using (var transaction = await context.BeginTransactionAsync())
                        {
                            try
                            {
                                await context.MailAddressCollectionItems
                                    .DeleteAsync(e =>
                                        e.CollectionId == existance.To ||
                                        (existance.Bcc != null && e.CollectionId == existance.Bcc) ||
                                        (existance.Cc != null && e.CollectionId == existance.Cc) ||
                                        (existance.ReplyTo != null && e.CollectionId == existance.ReplyTo)
                                    );

                                await context.MailAddressCollections.BulkCopyAsync(mailAddressCollections);
                                await context.MailAddressCollectionItems.BulkCopyAsync(mailAddressCollectionItems);

                                await context.Messages
                                    .Where(e => e.Id == entity.Id && e.Status == Enums.MessageStatus.Draft)
                                    .Set(e => e.Flags, entity.Flags)
                                    .Set(e => e.SubjectIsHtml, entity.SubjectIsHtml)
                                    .Set(e => e.Subject, entity.Subject)
                                    .Set(e => e.SubjectRAW, entity.SubjectRAW)
                                    .Set(e => e.BodyIsHtml, entity.BodyIsHtml)
                                    .Set(e => e.Body, entity.Body)
                                    .Set(e => e.From, entity.From)
                                    .Set(e => e.Headers, entity.Headers)
                                    .Set(e => e.Priority, entity.Priority)
                                    .Set(e => e.To, entity.To)
                                    .Set(e => e.Cc, entity.Cc)
                                    .Set(e => e.Bcc, entity.Bcc)
                                    .Set(e => e.ReplyTo, entity.ReplyTo)
                                    .UpdateAsync();

                                await transaction.CommitAsync();
                            }
                            catch
                            {
                                await transaction.RollbackAsync();
                                throw;
                            }
                        }
                    }
                }

                return Reply.Success();
            }
            catch (Exception e)
            {
                return Fail(request, nameof(Update), e);
            }
        }

        public async Task<Reply> Delete(Request<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Reply.SuccessWithWarning(I18n.Messages.Warning_NoRecordsAffected);

            try
            {
                await using (var context = GetDbContext())
                {
                    var id = await context.Messages
                        .Where(e => e.Id == request.Payload.Id || e.Serial == request.Payload.Serial)
                        .Select(e => e.Id)
                        .FirstOrDefaultAsync();


                    if (id != Guid.Empty)
                    {
                        await using (var transaction = await context.BeginTransactionAsync())
                        {
                            try
                            {
                                await context.Messages
                                    .Where(e => e.Id == id)
                                    .Set(e => e.Flags, e => e.Flags | DataSource.RecordFlag.Deleted)
                                    .UpdateAsync();

                                await transaction.CommitAsync();
                            }
                            catch
                            {
                                await transaction.RollbackAsync();
                                throw;
                            }
                        }
                    }
                }

                return Reply.Success();
            }
            catch (Exception e)
            {
                return Fail(request, nameof(Delete), e);
            }
        }

        public async Task<Reply<Core.Models.MessageModel?>> Get(Request<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Reply<Core.Models.MessageModel?>.SuccessWithWarning(null, message: I18n.Messages.Warning_RecordNotFound);

            try
            {
                DataSource.Tables.Message? entity = null;
                DataSource.Tables.MailAddressCollectionItem[]? mailAddresseItems = null;

                await using (var context = GetDbContext())
                {
                    entity = await context.Messages
                        .Where(e => e.Id == request.Payload.Id || e.Serial == request.Payload.Serial)
                        .Where(e => (e.Flags & DataSource.RecordFlag.Deleted) == 0)
                        .FirstOrDefaultAsync();

                    if (entity is not null)
                    {
                        mailAddresseItems = await context.MailAddressCollectionItems
                            .Where(e => e.CollectionId == entity.To ||
                                e.CollectionId == entity.Cc ||
                                e.CollectionId == entity.Bcc ||
                                e.CollectionId == entity.ReplyTo
                            ).ToArrayAsync();
                    }
                }


                return entity is null
                    ? Reply<Core.Models.MessageModel?>.SuccessWithWarning(data: null, I18n.Messages.Warning_RecordNotFound)
                    : Reply<Core.Models.MessageModel?>.Success(ToModel(entity, mailAddresseItems!));
            }
            catch (Exception e)
            {
                return Fail<Core.Models.MessageModel?>(request, nameof(Get), e);
            }
        }

        public async Task<ListReply<Core.Models.MessageModel>> List(Request request)
        {
            try
            {
                var pagination = PaginationInfo.Get(request.ListQuery);
                string? filterText = request.ListQuery?.Query.Clean()
                    , sortFiled = request.ListQuery?.SortBy?.Trim().ToLower();
                var totalItems = 0;

                DataSource.Tables.Message[] entities = [];
                DataSource.Tables.MailAddressCollectionItem[]? mailAddresseItems = null;

                await using (var context = GetDbContext())
                {
                    var query = context.Messages
                        .Where(e => (e.Flags & DataSource.RecordFlag.Deleted) == 0)
                        .Where(e => string.IsNullOrWhiteSpace(filterText) ||
                                    e.Subject.Contains(filterText) ||
                                    context.MailAddressCollectionItems.Any(x => x.CollectionId == e.To
                                        && (x.Address.Contains(filterText) || x.DisplayName.Contains(filterText)))
                            );

                    switch (sortFiled)
                    {
                        case "creationtime":
                            query = request.ListQuery!.DescendingSort
                                ? query.OrderByDescending(e => e.CreationTime)
                                : query.OrderBy(e => e.CreationTime);
                            break;
                        case "subject":
                            query = request.ListQuery!.DescendingSort
                                ? query.OrderByDescending(e => e.Subject)
                                : query.OrderBy(e => e.Subject);
                            break;
                        case "status":
                            query = request.ListQuery!.DescendingSort
                                ? query.OrderByDescending(e => e.Status)
                                : query.OrderBy(e => e.Status);
                            break;
                        default:
                            query = query.OrderByDescending(e => e.Serial);
                            break;
                    }

                    totalItems = await query.CountAsync();


                    if (pagination is not null)
                    {
                        query = query
                            .Skip(pagination.Skip)
                            .Take(pagination.PageSize);
                    }

                    mailAddresseItems = await context.MailAddressCollectionItems
                        .Where(e => query.Any(q => q.To == e.CollectionId ||
                            q.Cc == e.CollectionId ||
                            q.Bcc == e.CollectionId ||
                            q.ReplyTo == e.CollectionId)
                        ).ToArrayAsync();

                    entities = await query.Select(e => new DataSource.Tables.Message
                    {
                        Id = e.Id,
                        Serial = e.Serial,
                        Flags = e.Flags,
                        CreationTime = e.CreationTime,
                        AccountId = e.AccountId,
                        BodyIsHtml = e.BodyIsHtml,
                        SubjectIsHtml = e.SubjectIsHtml,
                        LastStatusTime = e.LastStatusTime,
                        Status = e.Status,
                        Priority = e.Priority,
                        From = e.From,
                        Subject = e.Subject,
                        SubjectRAW = e.SubjectRAW,
                        NumberOfTries = e.NumberOfTries,
                        Cc = e.Cc,
                        Bcc = e.Bcc,
                        To = e.To,
                        ReplyTo = e.ReplyTo,
                        Headers = e.Headers,
                        Body = "",
                    }).ToArrayAsync();
                }

                return ListReply<Core.Models.MessageModel>.Success(
                    data: entities.Select(e => ToModel(e, mailAddresseItems)).ToArray()!,
                    totalItems: totalItems,
                    pageNumber: request.ListQuery?.Page ?? 1,
                    pageSize: request.ListQuery?.PageSize ?? 1
                    );

            }
            catch (Exception e)
            {
                return ListFail<Core.Models.MessageModel>(request, nameof(List), e);
            }
        }

        public async Task<Reply<bool>> SetStatus(Request<Core.Models.MessageStatusModel> request)
        {
            try
            {
                var messageId = Guid.Empty;
                await using (var context = GetDbContext())
                {
                    messageId = await context.Messages
                                .Where(e => e.Id == request.Payload.Id || e.Serial == request.Payload.Serial)
                                .Select(e => e.Id)
                                .FirstOrDefaultAsync();

                    if (messageId != Guid.Empty)
                    {
                        await using (var transaction = await context.BeginTransactionAsync(System.Data.IsolationLevel.Serializable))
                        {
                            try
                            {
                                await context.Messages
                                    .Where(x => x.Id == messageId)
                                    .Set(x => x.Status, request.Payload.Status)
                                    .Set(x => x.LastStatusTime, request.When.UtcDateTime)
                                    .UpdateAsync();



                                if (request.Payload.AddLog)
                                {
                                    await context.MessageSendLogs.InsertAsync(() => new DataSource.Tables.MessageTrySendLog
                                    {
                                        Id = Guid.NewGuid(),
                                        MessageId = messageId,
                                        When = request.When.UtcDateTime,
                                        Status = request.Payload.Status,
                                        Description = string.IsNullOrWhiteSpace(request.Payload.Description) ? null : request.Payload.Description.Trim()
                                    });

                                    //await context.InsertAsync(new DataSource.Tables.MessageTrySendLog
                                    //{
                                    //    MessageId = messageId,
                                    //    When = request.When.UtcDateTime,
                                    //    Status = request.Payload.Status
                                    //}, schemaName: DataSource.DbSchema.Core);
                                }

                                await transaction.CommitAsync();
                            }
                            catch
                            {
                                await transaction.RollbackAsync();
                                throw;
                            }
                        }
                    }
                }

                return Reply<bool>.Success(messageId != Guid.Empty);
            }
            catch (Exception e)
            {
                return Fail<bool>(request, nameof(SetStatus), e);
            }
        }

        public async Task<Reply<Core.Models.MessageModel?>> GetItemToSend(Request<Core.Models.MessageGetHeadItem> request)
        {
            try
            {
                DataSource.Tables.Message? entity = null;
                DataSource.Tables.MailAddressCollectionItem[]? mailAddresseItems = null;

                await using (var context = GetDbContext())
                {
                    await using (var transaction = await context.BeginTransactionAsync(System.Data.IsolationLevel.Serializable))
                    {
                        try
                        {
                            entity = await context.Messages
                                .Where(e => e.Priority == request.Payload.Priority && (e.Flags & DataSource.RecordFlag.Deleted) == 0)
                                .Where(e => e.Status == Enums.MessageStatus.Pending || e.Status == Enums.MessageStatus.Failed)
                                .Where(e => e.NumberOfTries < request.Payload.MaxTry)
                                .OrderBy(e => e.Status)
                                .ThenBy(e => e.NumberOfTries)
                                .FirstOrDefaultAsync();

                            if (entity is not null)
                            {
                                await context.Messages
                                    .Where(e => e.Id == entity.Id)
                                    .Set(e => e.Status, Enums.MessageStatus.Sending)
                                    .Set(e => e.LastStatusTime, DateTime.UtcNow)
                                    .Set(e => e.NumberOfTries, e => e.NumberOfTries + 1)
                                    .UpdateAsync();
                            }

                            await transaction.CommitAsync();
                        }
                        catch
                        {
                            await transaction.RollbackAsync();

                            throw;
                        }
                    }

                    if (entity is not null)
                    {
                        mailAddresseItems = await context.MailAddressCollectionItems
                            .Where(e => e.CollectionId == entity.To ||
                                e.CollectionId == entity.Cc ||
                                e.CollectionId == entity.Bcc ||
                                e.CollectionId == entity.ReplyTo
                            ).ToArrayAsync();
                    }
                }


                return entity is null
                    ? Reply<Core.Models.MessageModel?>.SuccessWithWarning(data: null, I18n.Messages.Warning_RecordNotFound)
                    : Reply<Core.Models.MessageModel?>.Success(ToModel(entity, mailAddresseItems!));
            }
            catch (Exception e)
            {
                return Fail<Core.Models.MessageModel?>(request, nameof(Get), e);
            }
        }

        private Core.Models.MessageModel ToModel(DataSource.Tables.Message e, IEnumerable<DataSource.Tables.MailAddressCollectionItem> mailAddresses)
        {
            return ModelMapper.ToModel(entity: e,
                        to: mailAddresses?.Where(x => x.CollectionId == e.To).Select(x => new MailAddress(x.Address, x.DisplayNameRAW)).ToArray() ?? [],
                        cc: mailAddresses?.Where(x => x.CollectionId == e.Cc).Select(x => new MailAddress(x.Address, x.DisplayNameRAW)).ToArray(),
                        bcc: mailAddresses?.Where(x => x.CollectionId == e.Bcc).Select(x => new MailAddress(x.Address, x.DisplayNameRAW)).ToArray(),
                        replyTo: mailAddresses?.Where(x => x.CollectionId == e.ReplyTo).Select(x => new MailAddress(x.Address, x.DisplayNameRAW)).ToArray()
                        )!;
        }
    }
}
