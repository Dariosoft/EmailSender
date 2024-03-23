using System.Security.Cryptography;

namespace Dariosoft.EmailSender.Infrastructure.Database.Repositories
{
    internal class AccountRepository(RepositoryInjection injection) : Repository(injection), Core.Repositories.IAccountRepository
    {
        public async Task<Reply> Create(Request<Core.Models.AccountModel> request)
        {
            try
            {
                var entity = ModelMapper.ToEntity(request.Payload)!;

                await using (var context = GetDbContext())
                {
                    await context.InsertAsync(entity, schemaName: DataSource.DbSchema.Core);

                    request.Payload.Serial = entity.Serial = await context.Accounts.Where(e => e.Id == entity.Id)
                        .Select(e => e.Serial)
                        .FirstOrDefaultAsync();
                }

                return Reply.Success();
            }
            catch (Exception e)
            {
                return Fail(request, nameof(Create), e);
            }
        }

        public async Task<Reply> Update(Request<Core.Models.AccountModel> request)
        {
            try
            {
                var entity = ModelMapper.ToEntity(request.Payload)!;

                await using (var context = GetDbContext())
                {
                    var id = entity.Id == Guid.Empty && entity.Serial > 0
                        ? await context.Accounts
                        .Where(e => e.Id == entity.Id || e.Serial == entity.Serial)
                        .Select(e => e.Id)
                        .FirstOrDefaultAsync()
                        : Guid.Empty;

                    if (id != Guid.Empty)
                    {
                        await context.Accounts
                            .Where(e => e.Id == id)
                            .Set(e => e.Flags, entity.Flags)
                            .Set(e => e.HostId, entity.HostId)
                            .Set(e => e.EmailAddress, entity.EmailAddress)
                            .Set(e => e.Password, entity.Password)
                            .Set(e => e.DisplayName, entity.DisplayName)
                            .Set(e => e.DisplayNameRAW, entity.DisplayNameRAW)
                            .Set(e => e.Description, entity.Description)
                            .Set(e => e.DescriptionRAW, entity.DescriptionRAW)
                            .UpdateAsync();
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
                    var id = await context.Accounts
                        .Where(e => e.Id == request.Payload.Id || e.Serial == request.Payload.Serial)
                        .Select(e => e.Id)
                        .FirstOrDefaultAsync();


                    if (id != Guid.Empty)
                    {
                        await using (var transaction = await context.BeginTransactionAsync())
                        {
                            try
                            {
                                await context.Accounts
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

        public async Task<Reply<Core.Models.AccountModel?>> Get(Request<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Reply<Core.Models.AccountModel?>.SuccessWithWarning(null, message: I18n.Messages.Warning_RecordNotFound);

            try
            {
                DataSource.Tables.Account? entity = null;

                await using (var context = GetDbContext())
                {
                    entity = await context.Accounts
                        .Where(e => e.Id == request.Payload.Id || e.Serial == request.Payload.Serial)
                        .Where(e => (e.Flags & DataSource.RecordFlag.Deleted) == 0)
                        .FirstOrDefaultAsync();
                }

                return entity is null
                    ? Reply<Core.Models.AccountModel?>.SuccessWithWarning(data: null, I18n.Messages.Warning_RecordNotFound)
                    : Reply<Core.Models.AccountModel?>.Success(ModelMapper.ToModel(entity));
            }
            catch (Exception e)
            {
                return Fail<Core.Models.AccountModel?>(request, nameof(Get), e);
            }
        }

        public async Task<Reply> SetAvailability(Request<Core.Models.SetAvailabilityModel> request)
        {
            try
            {
                await using (var context = GetDbContext())
                {
                    await context.Accounts
                        .Where(e => e.Id == request.Payload.Id || e.Serial == request.Payload.Serial)
                        .Set(e => e.Flags, e => request.Payload.Enabled
                            ? e.Flags & ~DataSource.RecordFlag.Deleted
                            : e.Flags | DataSource.RecordFlag.Deleted
                            )
                        .UpdateAsync();
                }

                return Reply.Success();
            }
            catch (Exception e)
            {
                return Fail(request, nameof(SetAvailability), e);
            }
        }

        public async Task<ListReply<Core.Models.AccountModel>> List(Request request)
        {
            try
            {
                var pagination = PaginationInfo.Get(request.ListQuery);
                string? filterText = request.ListQuery?.Query.Clean(),
                       sortFiled = request.ListQuery?.SortBy?.Trim().ToLower();
                var totalItems = 0;

                Core.Models.AccountModel[] items = [];

                await using (var context = GetDbContext())
                {
                    var query = context.Accounts
                        .Where(e => (e.Flags & DataSource.RecordFlag.Deleted) == 0)
                        .Where(e => string.IsNullOrWhiteSpace(filterText) ||
                            e.EmailAddress.Contains(filterText) ||
                            e.DisplayName.Contains(filterText) ||
                            e.Description.Contains(filterText)
                        );

                    switch (sortFiled)
                    {
                        case "creationtime":
                            query = request.ListQuery!.DescendingSort
                                ? query.OrderByDescending(e => e.CreationTime)
                                : query.OrderBy(e => e.CreationTime);
                            break;
                        case "username":
                            query = request.ListQuery!.DescendingSort
                                ? query.OrderByDescending(e => e.EmailAddress)
                                : query.OrderBy(e => e.EmailAddress);
                            break;
                        case "displayname":
                            query = request.ListQuery!.DescendingSort
                                ? query.OrderByDescending(e => e.DisplayName)
                                : query.OrderBy(e => e.DisplayName);
                            break;
                        case "description":
                            query = request.ListQuery!.DescendingSort
                                ? query.OrderByDescending(e => e.Description)
                                : query.OrderBy(e => e.Description);
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

                    items = await (from a in query
                                 from h in context.Hosts.InnerJoin(e => e.Id == a.HostId)
                                 from c in context.Clients.InnerJoin(e => e.Id == a.ClientId)
                                 select new Core.Models.AccountModel
                                 {
                                     Id = a.Id,
                                     Serial = a.Serial,
                                     CreationTime = a.CreationTime,
                                     Enabled = (a.Flags & DataSource.RecordFlag.Disable) == 0,
                                     ClientId = a.ClientId,
                                     ClientName = c.NameRAW,
                                     HostId = a.HostId,
                                     Host = h.Address,
                                     EmailAddress = a.EmailAddress,
                                     Password = "",
                                     DisplayName = a.DisplayNameRAW,
                                     Description = a.DescriptionRAW,
                                 }
                                 ).ToArrayAsync();
                }

                return ListReply<Core.Models.AccountModel>.Success(
                    data: items,
                    totalItems: totalItems,
                    pageNumber: request.ListQuery?.Page ?? 1,
                    pageSize: request.ListQuery?.PageSize ?? 1
                    );

            }
            catch (Exception e)
            {
                return ListFail<Core.Models.AccountModel>(request, nameof(List), e);
            }
        }
    }
}
