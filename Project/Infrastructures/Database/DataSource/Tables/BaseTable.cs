namespace Dariosoft.EmailSender.Infrastructure.Database.DataSource.Tables
{
    internal abstract record BaseTable
    {
        [Column(IsPrimaryKey = true, SkipOnUpdate = true)]
        public Guid Id { get; set; }

        [Column(IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)]
        public int Serial { get; set; }
    }
}
