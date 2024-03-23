namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models.Client
{
    public record CreateClientModel
    {
        public bool Enabled { get; set; }

        public required string AdminUserName { get; set; }
        
        public required string AdminPassword { get; set; }
        
        public required string Name { get; set; }

        public string? Description { get; set; }

        public override string ToString() => Name;
    }
}
