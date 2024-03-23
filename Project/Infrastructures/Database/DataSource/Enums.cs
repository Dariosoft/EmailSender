namespace Dariosoft.EmailSender.Infrastructure.Database.DataSource
{
    enum RecordFlag: int
    {
        None = 0,
        Temporary = 1,
        Default = 2,
        Disable = 4,
        Deleted = 8,

    }
}
