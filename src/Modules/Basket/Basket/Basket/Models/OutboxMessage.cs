﻿namespace Basket.Basket.Models
{
    public class OutboxMessage : Entity<Guid>
    {
        public string Type { get; set; } = default!;
        public string Data { get; set; } = default!;
        public DateTime OccurredOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
    }
}
