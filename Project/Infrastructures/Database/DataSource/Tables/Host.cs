using System.Net;
using System.Net.Mail;

namespace Dariosoft.EmailSender.Infrastructure.Database.DataSource.Tables
{
    internal record Host : FlaggedTable
    {
        public Guid? ClientId { get; set; }

        public required string Address { get; set; }

        public required int PortNumber { get; set; }

        public required bool UseSsl { get; set; }

        public string? Description { get; set; }

        public string? DescriptionRAW { get; set; }

        public override string ToString() => Address;
    }
}
