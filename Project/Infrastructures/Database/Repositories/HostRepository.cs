namespace Dariosoft.EmailSender.Infrastructure.Database.Repositories
{
    internal class HostRepository(RepositoryInjection injection) : Repository(injection), Core.Repositories.IHostRepository
    {
        public async Task<Reply> Create(Request<Core.Models.HostModel> request)
        {
            try
            {
                var entity = ModelMapper.ToEntity(request.Payload)!;

                await using (var context = GetDbContext())
                {
                    await context.InsertAsync(entity, schemaName: DataSource.DbSchema.Core);

                    request.Payload.Serial = entity.Serial = await context.Hosts.Where(e => e.Id == entity.Id)
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

        public async Task<Reply> Update(Request<Core.Models.HostModel> request)
        {
            try
            {
                var entity = ModelMapper.ToEntity(request.Payload)!;

                await using (var context = GetDbContext())
                {
                    var id = entity.Id == Guid.Empty && entity.Serial > 0
                        ? await context.Hosts
                        .Where(e => e.Id == entity.Id || e.Serial == entity.Serial)
                        .Select(e => e.Id)
                        .FirstOrDefaultAsync()
                        : Guid.Empty;

                    if (id != Guid.Empty)
                    {
                        await context.Hosts
                            .Where(e => e.Id == id)
                            .Set(e => e.Flags, entity.Flags)
                            .Set(e => e.Address, entity.Address)
                            .Set(e => e.PortNumber, entity.PortNumber)
                            .Set(e => e.UseSsl, entity.UseSsl)
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
                    var id = await context.Hosts
                        .Where(e => e.Id == request.Payload.Id || e.Serial == request.Payload.Serial)
                        .Select(e => e.Id)
                        .FirstOrDefaultAsync();


                    if (id != Guid.Empty)
                    {
                        await using (var transaction = await context.BeginTransactionAsync())
                        {
                            try
                            {
                                await context.Hosts
                                    .Where(e => e.Id == id)
                                    .Set(e => e.Flags, e => e.Flags | DataSource.RecordFlag.Deleted)
                                    .UpdateAsync();

                                await context.Accounts
                                    .Where(e => e.HostId == id)
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

        public async Task<Reply<Core.Models.HostModel?>> Get(Request<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Reply<Core.Models.HostModel?>.SuccessWithWarning(null, message: I18n.Messages.Warning_RecordNotFound);

            try
            {
                DataSource.Tables.Host? entity = null;

                await using (var context = GetDbContext())
                {
                    entity = await context.Hosts
                        .Where(e => e.Id == request.Payload.Id || e.Serial == request.Payload.Serial)
                        .Where(e => (e.Flags & DataSource.RecordFlag.Deleted) == 0)
                        .FirstOrDefaultAsync();
                }

                return entity is null
                    ? Reply<Core.Models.HostModel?>.SuccessWithWarning(data: null, I18n.Messages.Warning_RecordNotFound)
                    : Reply<Core.Models.HostModel?>.Success(ModelMapper.ToModel(entity));
            }
            catch (Exception e)
            {
                return Fail<Core.Models.HostModel?>(request, nameof(Get), e);
            }
        }

        public async Task<Reply> SetAvailability(Request<Core.Models.SetAvailabilityModel> request)
        {
            try
            {
                await using (var context = GetDbContext())
                {
                    await context.Hosts
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

        public async Task<ListReply<Core.Models.HostModel>> List(Request request)
        {
            try
            {
                var pagination = PaginationInfo.Get(request.ListQuery);
                string? filterText = request.ListQuery?.Query.Clean(),
                       sortFiled = request.ListQuery?.SortBy?.Trim().ToLower();
                var totalItems = 0;

                Core.Models.HostModel[] items = [];

                await using (var context = GetDbContext())
                {
                    var query = context.Hosts
                        .Where(e => (e.Flags & DataSource.RecordFlag.Deleted) == 0)
                        .Where(e => string.IsNullOrWhiteSpace(filterText) ||
                            e.Address.Contains(filterText) ||
                            e.Description.Contains(filterText)
                        );

                    switch (sortFiled)
                    {
                        case "creationtime":
                            query = request.ListQuery!.DescendingSort
                                ? query.OrderByDescending(e => e.CreationTime)
                                : query.OrderBy(e => e.CreationTime);
                            break;
                        case "address":
                            query = request.ListQuery!.DescendingSort
                                ? query.OrderByDescending(e => e.Address)
                                : query.OrderBy(e => e.Address);
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

                    items = await (from h in query
                                   from c in context.Clients.LeftJoin(e => e.Id == h.ClientId)
                                   select new Core.Models.HostModel {
                                       Id = h.Id,
                                       Serial = h.Serial,
                                       CreationTime = h.CreationTime,
                                       Enabled = (h.Flags & DataSource.RecordFlag.Deleted) == 0,
                                       ClientId = h.ClientId,
                                       ClientName = c.NameRAW,
                                       Address = h.Address,
                                       PortNumber = h.PortNumber,
                                       UseSsl = h.UseSsl,
                                       Description = h.DescriptionRAW
                                   }
                                   ).ToArrayAsync();
                }

                return ListReply<Core.Models.HostModel>.Success(
                    data: items,
                    totalItems: totalItems,
                    pageNumber: request.ListQuery?.Page ?? 1,
                    pageSize: request.ListQuery?.PageSize ?? 1
                    );

            }
            catch (Exception e)
            {
                return ListFail<Core.Models.HostModel>(request, nameof(List), e);
            }
        }
    }
}
