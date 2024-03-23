namespace Dariosoft.EmailSender.Infrastructure.Database.DataSource.Tables
{

    internal record Client : FlaggedTable
    {

        // [Column(CanBeNull = false, Length = 128)]
        public required string Name { get; set; } = "";

        // [Column(CanBeNull = false, Length = 128)]
        public required string NameRAW { get; set; } = "";

        //[Column(CanBeNull = true, Length = 256)]
        public string? ApiKey { get; set; }

        //[Column(CanBeNull = true, Length = 256)]
        public string? Description { get; set; }

        // [Column(CanBeNull = true, Length = 256)]
        public string? DescriptionRAW { get; set; }

        public override string ToString() => NameRAW;
    }
}
