﻿namespace Dariosoft.EmailSender.Core.Models
{
    public abstract record BaseModel
    {
        public required Guid Id { get; set; }

        public required int Serial { get; set; }

        public required DateTimeOffset CreationTime { get; set; }
    }
}
