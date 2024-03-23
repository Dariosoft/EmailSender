namespace Dariosoft.EmailSender.Core.Models
{
    public record ClientModel : BaseModel
    {
        public bool Enabled { get; set; }

        public required string AdminUserName { get; set; }
        
        public required string AdminPassword { get; set; }
        
        public required string Name { get; set; }

        public string ApiKey { get; set; } = "";

        public string? Description { get; set; }

        public override string ToString() => Name;


    }

    public record CreateClientModel
    {
        public bool Enabled { get; set; }

        public required string AdminUserName { get; set; }

        public required string AdminPassword { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public override string ToString() => Name;
    }

    public record UpdateClientModel
    {
        public required KeyModel Key { get; set; }

        public bool Enabled { get; set; }

        public required string AdminUserName { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public override string ToString() => Name;
    }
}
