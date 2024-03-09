namespace Dariosoft.EmailSender.EndPoint
{

    public static class ModelExtensions
    {

        #region HostModel
        public static Core.Models.HostModel Patch(this Abstraction.Models.Host.CreateHostModel model)
            => new Core.Models.HostModel
            {
                Id = Guid.NewGuid(),
                Serial = 0,
                CreationTime = DateTimeOffset.UtcNow,
                Enabled = model.Enabled,
                Address = model.Address,
                PortNumber = model.PortNumber,
                UseSsl = model.UseSsl,
                Description = model.Description,
            };

        public static Core.Models.HostModel Patch(this Abstraction.Models.Host.UpdateHostModel model)
        {
            Core.Models.KeyModel key = model.Key;

            return new Core.Models.HostModel
            {
                Id = key.Id,
                Serial = key.Serial,
                CreationTime = default,
                Enabled = model.Enabled,
                Address = model.Address,
                PortNumber = model.PortNumber,
                UseSsl = model.UseSsl,
                Description = model.Description,
            };
        }

        public static Abstraction.Models.Host.HostModel Unpatch(this Core.Models.HostModel model)
            => new Abstraction.Models.Host.HostModel
            {
                Id = model.Id.ToString().ToLower(),
                Serial = model.Serial,
                CreationTime = model.CreationTime,
                Enabled = model.Enabled,
                Address = model.Address,
                PortNumber = model.PortNumber,
                UseSsl = model.UseSsl,
                Description = model.Description,
            };
        #endregion

        public static Framework.ListQueryModel Patch(this Abstraction.Models.Common.ListQueryModel model)
            => new Framework.ListQueryModel
            {
                Page = model.Page,
                PageSize = model.PageSize,
                Query = model.Query,
                DescendingSort = model.DescendingSort,
                SortBy = model.SortBy,
                Parameters = model.Parameters
            };

        public static Core.Models.SetAvailabilityModel Patch(this Abstraction.Models.Common.SetAvailabilityModel model)
        {
            Core.Models.KeyModel key = model.Key;

            return new Core.Models.SetAvailabilityModel
            {
                Id = key.Id,
                Serial = key.Serial,
                Enabled = model.Enabled
            };
        }

    }
}
