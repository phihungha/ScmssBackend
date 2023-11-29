﻿namespace ScmssApiServer.Models
{
    public abstract class Goods : IUpdateTrackable
    {
        public DateTime CreateTime { get; set; }
        public string? Description { get; set; }
        public int ExpirationMonth { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }

        public ICollection<ProductionFacility> ProductionFacilities { get; set; }
            = new List<ProductionFacility>();

        public required string Unit { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
