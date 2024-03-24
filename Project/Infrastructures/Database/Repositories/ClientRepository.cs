namespace Dariosoft.EmailSender.Infrastructure.Database.Repositories
{
    internal class ClientRepository(RepositoryInjection injection) : Repository(injection), Core.Repositories.IClientRepository
    {
        public async Task<IResponse> Create(IRequest<Core.Models.ClientModel> request)
        {
            try
            {
                var entity = ModelMapper.ToEntity(request.Payload)!;
                var admin = ModelMapper.ToAdminEntity(request.Payload)!;

                await using (var context = GetDbContext())
                {
                    await using (var transaction = await context.BeginTransactionAsync())
                    {
                        try
                        {
                            await context.InsertAsync(admin, schemaName: DataSource.DbSchema.Core);
                            await context.InsertAsync(entity, schemaName: DataSource.DbSchema.Core);
                            await context.CommitTransactionAsync();
                        }
                        catch
                        {
                            await context.RollbackTransactionAsync();
                            throw;
                        }
                    }

                    request.Payload.Serial = entity.Serial = await context.Clients.Where(e => e.Id == entity.Id)
                        .Select(e => e.Serial)
                        .FirstOrDefaultAsync();
                }

                return Response.Success();
            }
            catch (Exception e)
            {
                return Fail(request, nameof(Create), e);
            }
        }

        public async Task<IResponse> Update(IRequest<Core.Models.ClientModel> request)
        {
            try
            {
                var entity = ModelMapper.ToEntity(request.Payload)!;

                await using (var context = GetDbContext())
                {
                    var id = entity.Id == Guid.Empty && entity.Serial > 0
                        ? await context.Clients
                        .Where(e => e.Id == entity.Id || e.Serial == entity.Serial)
                        .Select(e => e.Id)
                        .FirstOrDefaultAsync()
                        : Guid.Empty;

                    if (id != Guid.Empty)
                    {
                        await using (var transaction = await context.BeginTransactionAsync())
                        {
                            try
                            {
                                await context.Clients
                                    .Where(e => e.Id == id)
                                    .Set(e => e.Flags, entity.Flags)
                                    .Set(e => e.Name, entity.Name)
                                    .Set(e => e.NameRAW, entity.NameRAW)
                                    .Set(e => e.Description, entity.Description)
                                    .Set(e => e.DescriptionRAW, entity.DescriptionRAW)
                                    .UpdateAsync();

                                await context.Admins
                                    .Where(e => e.Id == entity.Id)
                                    .Set(e => e.Title, entity.Name)
                                    .Set(e => e.TitleRAW, entity.NameRAW)
                                    .Set(e => e.UserName, request.Payload.AdminUserName.Trim().ToLower())
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

                return Response.Success();
            }
            catch (Exception e)
            {
                return Fail(request, nameof(Update), e);
            }
        }

        public async Task<IResponse> Delete(IRequest<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Response.SuccessWithWarning(I18n.Messages.Warning_NoRecordsAffected);

            try
            {
                await using (var context = GetDbContext())
                {
                    var id = await context.Clients
                        .Where(e => e.Id == request.Payload.Id || e.Serial == request.Payload.Serial)
                        .Select(e => e.Id)
                        .FirstOrDefaultAsync();


                    if (id != Guid.Empty)
                    {
                        await using (var transaction = await context.BeginTransactionAsync())
                        {
                            try
                            {
                                await context.Clients
                                    .Where(e => e.Id == id)
                                    .Set(e => e.Flags, e => e.Flags | DataSource.RecordFlag.Deleted)
                                    .UpdateAsync();

                                await context.Admins
                                    .Where(e => e.Id == id)
                                    .Set(e => e.Flags, e => e.Flags | DataSource.RecordFlag.Deleted)
                                    .UpdateAsync();

                                await context.Hosts
                                    .Where(e => e.ClientId == id)
                                    .Set(e => e.Flags, e => e.Flags | DataSource.RecordFlag.Deleted)
                                    .UpdateAsync();

                                await context.Accounts
                                    .Where(e => e.ClientId == id)
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

                return Response.Success();
            }
            catch (Exception e)
            {
                return Fail(request, nameof(Delete), e);
            }
        }

        public async Task<IResponse<Core.Models.ClientModel?>> Get(IRequest<Core.Models.KeyModel> request)
        {
            if (request.Payload is null)
                return Response<Core.Models.ClientModel?>.SuccessWithWarning(null, message: I18n.Messages.Warning_RecordNotFound);

            try
            {
                DataSource.Tables.Client? entity = null;
                DataSource.Tables.Admin? admin = null;

                await using (var context = GetDbContext())
                {
                    entity = await context.Clients
                        .Where(e => e.Id == request.Payload.Id || e.Serial == request.Payload.Serial)
                        .Where(e => (e.Flags & DataSource.RecordFlag.Deleted) == 0)
                        .FirstOrDefaultAsync();

                    if (entity is not null)
                        admin = await context.Admins
                            .Where(e => e.Id == entity.Id)
                            .Where(e => (e.Flags & DataSource.RecordFlag.Deleted) == 0)
                            .FirstOrDefaultAsync();
                }

                return entity is null
                    ? Response<Core.Models.ClientModel?>.SuccessWithWarning(data: null, I18n.Messages.Warning_RecordNotFound)
                    : Response<Core.Models.ClientModel?>.Success(ModelMapper.ToModel(entity, admin));
            }
            catch (Exception e)
            {
                return Fail<Core.Models.ClientModel?>(request, nameof(Get), e);
            }
        }

        public async Task<IResponse> SetAvailability(IRequest<Core.Models.SetAvailabilityModel> request)
        {
            try
            {
                await using (var context = GetDbContext())
                {
                    var id = await context.Clients
                        .Where(e => e.Id == request.Payload.Id || e.Serial == request.Payload.Serial)
                        .Select(e => e.Id)
                        .FirstOrDefaultAsync();

                    if (id != Guid.Empty)
                    {
                        await using (var transaction = await context.BeginTransactionAsync())
                        {
                            try
                            {
                                await context.Clients
                                    .Where(e => e.Id == id)
                                    .Set(e => e.Flags, e => request.Payload.Enabled
                                        ? e.Flags & ~DataSource.RecordFlag.Disable
                                        : (e.Flags | DataSource.RecordFlag.Disable)
                                        )
                                    .UpdateAsync();

                                await context.Admins
                                    .Where(e => e.Id == id)
                                    .Set(e => e.Flags, e => request.Payload.Enabled
                                        ? e.Flags & ~DataSource.RecordFlag.Disable
                                        : (e.Flags | DataSource.RecordFlag.Disable)
                                        )
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

                return Response.Success();
            }
            catch (Exception e)
            {
                return Fail(request, nameof(SetAvailability), e);
            }
        }

        public async Task<IListResponse<Core.Models.ClientModel>> List(IRequest request)
        {
            try
            {
                var pagination = PaginationInfo.Get(request.ListQuery);
                string? filterText = request.ListQuery?.Query.Clean(),
                       sortFiled = request.ListQuery?.SortBy?.Trim().ToLower();
                var totalItems = 0;

                DataSource.Tables.Client[] entities = [];
                DataSource.Tables.Admin[] admins = [];

                await using (var context = GetDbContext())
                {
                    var query = context.Clients
                        .Where(e => (e.Flags & DataSource.RecordFlag.Deleted) == 0)
                        .Where(e => string.IsNullOrWhiteSpace(filterText) ||
                            e.Name.Contains(filterText) ||
                            e.Description.Contains(filterText)
                        );

                    switch (sortFiled)
                    {
                        case "creationtime":
                            query = request.ListQuery!.DescendingSort
                                ? query.OrderByDescending(e => e.CreationTime)
                                : query.OrderBy(e => e.CreationTime);
                            break;
                        case "name":
                            query = request.ListQuery!.DescendingSort
                                ? query.OrderByDescending(e => e.Name)
                                : query.OrderBy(e => e.Name);
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

                    entities = await query.ToArrayAsync();

                    admins = await context.Admins
                        .Where(e => query.Any(q => q.Id == e.Id))
                        .Select(e => new DataSource.Tables.Admin { Id = e.Id, UserName = e.UserName })
                        .ToArrayAsync();
                }

             

                return ListResponse<Core.Models.ClientModel>.Success(
                    data: entities.Join(admins, e => e.Id, a => a.Id, (e, a) => ModelMapper.ToModel(e, a)).ToArray()!,
                    totalItems: totalItems,
                    pageNumber: request.ListQuery?.Page ?? 1,
                    pageSize: request.ListQuery?.PageSize ?? 1
                    );

            }
            catch (Exception e)
            {
                return ListFail<Core.Models.ClientModel>(request, nameof(List), e);
            }
        }
    }
}
