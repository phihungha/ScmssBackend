﻿using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class ProductionOrderEventCreateDto : OrderEventCreateDto
    {
        [Required]
        public ProductionOrderEventTypeSelection Type { get; set; }

        [Required]
        public required string Location { get; set; }
    }
}
