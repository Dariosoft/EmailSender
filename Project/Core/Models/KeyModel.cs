
namespace Dariosoft.EmailSender.Core.Models
{
    public record KeyModel
    {
        public KeyModel()
        {

        }

        public KeyModel(string key)
        {
            if (Guid.TryParse(key, out var id))
                Id = id;
            else if (int.TryParse(key, out var serial))
                Serial = serial;
        }

        public Guid Id { get; set; }

        public int Serial { get; set; }

        public static implicit operator KeyModel(string key) => new KeyModel(key);
        public static implicit operator KeyModel(Guid key) => new KeyModel { Id = key };
        public static implicit operator KeyModel(int key) => new KeyModel { Serial = key };
    }
}
