namespace Dariosoft.EmailSender.Infrastructure.Database.DataSource.Tables
{
    internal record Admin : FlaggedTable
    {
        public bool IsSuperAdmin { get; set; }

        public string Title { get; set; } = "";

        public string TitleRAW { get; set; } = "";

        public string UserName { get; set; } = "";

        public string Password { get; set; } = "";
    }
}
