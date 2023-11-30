﻿using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public abstract class TransOrderDto : OrderDto
    {
        public ICollection<TransOrderEventDto> Events { get; set; }
            = new List<TransOrderEventDto>();

        public string? InvoiceUrl { get; set; }
        public TransOrderPaymentStatus PaymentStatus { get; set; }
        public string? ReceiptUrl { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal VatAmount { get; set; }
        public double VatRate { get; set; }
    }
}
