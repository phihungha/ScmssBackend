﻿namespace ScmssApiServer.DTOs
{
    public class PurchaseOrderDto : TransOrderDto<PurchaseOrderItemDto>
    {
        public decimal AdditionalDiscount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountSubtotal { get; set; }
        public Uri? InvoiceUrl { get; set; }
        public decimal NetSubtotal { get; set; }
        public required ProductionFacilityDto ProductionFacility { get; set; }
        public int ProductionFacilityId { get; set; }
        public required PurchaseRequisitionDto PurchaseRequisition { get; set; }
        public int? PurchaseRequisitionId { get; set; }
        public Uri? ReceiptUrl { get; set; }
        public CompanyDto Vendor { get; set; } = null!;
        public int VendorId { get; set; }
    }
}
