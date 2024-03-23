namespace Dariosoft.EmailSender.Application
{
    internal static class ModelExtensions
    {
        public static Core.Models.BaseModel TrimToBaseModel(this Core.Models.BaseModel model)
         => new Core.Models.BaseModel
         {
             Id = model.Id,
             Serial = model.Serial,
             CreationTime = model.CreationTime
         };
    }
}
