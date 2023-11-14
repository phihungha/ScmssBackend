﻿namespace ScmssApiServer.Models
{
    public class Supply : Goods
    {
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; } = null!;

        public ICollection<ProductionCostItem> ProductionCostItems { get; set; }
            = new List<ProductionCostItem>();

        public ICollection<Product> Products { get; set; }
            = new List<Product>();

        public ICollection<PurchaseRequisition> PurchaseRequisitions { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<PurchaseRequisitionItem> PurchaseRequisitionItems { get; set; }
            = new List<PurchaseRequisitionItem>();

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
            = new List<PurchaseOrder>();

        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
            = new List<PurchaseOrderItem>();

        public ICollection<WarehouseSupplyItem> WarehouseSupplyItems { get; set; }
            = new List<WarehouseSupplyItem>();
    }
}