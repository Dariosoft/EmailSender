namespace Dariosoft.EmailSender.Infrastructure.Database.DataSource.Tables
{

    internal abstract record FlaggedTable: BaseTable
    {
        // [Column(CanBeNull = false, DbType = "TIMESTAMP WITHOUT TIME ZONE")]
        public DateTime CreationTime { get; set; }

        // [Column(CanBeNull = false)]
        public RecordFlag Flags { get; set; }

    }
}
