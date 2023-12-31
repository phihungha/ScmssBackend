﻿namespace ScmssApiServer.DTOs
{
    public class SalesOrderDto : TransOrderDto<SalesOrderItemDto>
    {
        public CompanyDto Customer { get; set; } = null!;
        public int CustomerId { get; set; }
        public ProductionFacilityDto? ProductionFacility { get; set; }
        public int? ProductionFacilityId { get; set; }
    }
}
