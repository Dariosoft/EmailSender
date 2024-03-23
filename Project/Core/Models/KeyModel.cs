
namespace Dariosoft.EmailSender.Core.Models
{
    public record KeyModel
    {
        public KeyModel()
        {

        }

        public KeyModel(string? key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                if (Guid.TryParse(key, out var id))
                    Id = id;
                else if (int.TryParse(key, out var serial))
                    Serial = serial; 
            }
        }

        public Guid Id { get; set; }

        public int Serial { get; set; }

        public bool HasValue() => Id != Guid.Empty || Serial > 0;

        //public static implicit operator KeyModel(string key) => new KeyModel(key);
        public static implicit operator KeyModel?(string? key) => string.IsNullOrWhiteSpace(key) ? null : new KeyModel(key);
        public static implicit operator KeyModel(Guid key) => new KeyModel { Id = key };
        public static implicit operator KeyModel(int key) => new KeyModel { Serial = key };
    }
}
