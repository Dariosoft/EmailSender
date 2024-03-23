namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models.Client
{
    public record ClientModel
    {
        public required string Id { get; set; }

        public required int Serial { get; set; }

        public required DateTimeOffset CreationTime { get; set; }

        public bool Enabled { get; set; }

        public required string Name { get; set; }

        public required string AdminUserName { get; set; }

        public string AccessKey { get; set; } = "";

        public string? Description { get; set; }

        public override string ToString() => Name;
    }
}
