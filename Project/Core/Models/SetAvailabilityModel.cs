
namespace Dariosoft.EmailSender.Core.Models
{

    public record SetAvailabilityModel : KeyModel
    {
        public required bool Enabled { get; set; }
    }
}
