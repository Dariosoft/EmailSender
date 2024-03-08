namespace Dariosoft.EmailSender.EndPoint
{
    public static class ModelExtensions
    {
        public static Core.Models.HostModel ToHostModel(this Abstraction.Models.Host.CreateHostModel model)
            => new Core.Models.HostModel
            {
                Id = Guid.NewGuid(),
                Serial = 0,
                Enabled = model.Enabled,
                Address = model.Address,
                PortNumber = model.PortNumber,
                UseSsl = model.UseSsl,
                Description = model.Description,
            };

    }
}
