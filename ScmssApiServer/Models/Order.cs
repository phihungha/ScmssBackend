﻿using ScmssApiServer.DomainExceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public abstract class Order<TItem, TEvent> : ILifecycle
        where TItem : OrderItem
        where TEvent : OrderEvent, new()
    {
        public int Id { get; set; }

        public ICollection<TItem> Items { get; } = new List<TItem>();

        public decimal SubTotal { get; private set; }
        public double VatRate { get; private set; }
        public decimal VatAmount { get; private set; }
        public decimal TotalAmount { get; private set; }

        public abstract string? FromLocation { get; set; }
        public abstract string ToLocation { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Processing;
        public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;

        public string? InvoiceUrl { get; set; }
        public string? ReceiptUrl { get; set; }

        public ICollection<TEvent> Events { get; } = new List<TEvent>();

        public required string CreateUserId { get; set; }
        public User CreateUser { get; set; } = null!;
        public string? FinishUserId { get; private set; }
        public User? FinishUser { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateTime { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdateTime { get; set; }

        public DateTime? DeliverTime { get; protected set; }
        public DateTime? FinishTime { get; private set; }

        public void AddItem(TItem item)
        {
            bool idAlreadyExists = Items.FirstOrDefault(
                    i => i.ItemId == item.ItemId
                ) != null;
            if (idAlreadyExists)
            {
                throw new InvalidDomainOperationException("An order item with this ID already exists");
            }
            Items.Add(item);
            CalculateTotals();
        }

        public void AddManualEvent(OrderEventTypeSelection typeSel, string location, string? message)
        {
            OrderEventType type;
            switch (typeSel)
            {
                case OrderEventTypeSelection.Left:
                    type = OrderEventType.Left;
                    break;

                case OrderEventTypeSelection.Arrived:
                    type = OrderEventType.Arrived;
                    break;

                case OrderEventTypeSelection.Delivered:
                    type = OrderEventType.Delivered;
                    break;

                case OrderEventTypeSelection.Interrupted:
                    type = OrderEventType.Interrupted;
                    break;

                default:
                    throw new ArgumentException("Invalid event type");
            }
            AddEvent(type, location, message);
        }

        public void EditEvent(int id, string? location = null, string? message = null)
        {
            TEvent? item = Events.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                throw new EntityNotFoundException();
            }

            if (location != null)
            {
                if (item.Type != OrderEventType.Left
                || item.Type != OrderEventType.Arrived
                || item.Type != OrderEventType.Interrupted)
                {
                    throw new InvalidDomainOperationException("Cannot edit location of automatic event.");
                }
                item.Location = location;
            }
            item.Message = message;
        }

        public void Complete(string userId)
        {
            if (Status != OrderStatus.Delivered)
            {
                throw new InvalidDomainOperationException(
                    "Cannot complete order if it hasn't finished delivery or was canceled/returned."
                    );
            }
            Status = OrderStatus.Completed;
            PaymentStatus = OrderPaymentStatus.Due;
            Finish(userId);
            AddEvent(OrderEventType.Completed, ToLocation);
        }

        public void FinishDelivery()
        {
            if (Status != OrderStatus.Delivering)
            {
                throw new InvalidDomainOperationException(
                    "Cannot finish delivery of order if it is not being delivered."
                    );
            }
            Status = OrderStatus.Delivered;
            PaymentStatus = OrderPaymentStatus.Due;
            DeliverTime = DateTime.UtcNow;
        }

        public void Cancel(string userId)
        {
            if (Status == OrderStatus.Delivered || Status == OrderStatus.Completed)
            {
                throw new InvalidOperationException(
                    "Cannot cancel order after it has been delivered."
                    );
            }
            Status = OrderStatus.Canceled;
            PaymentStatus = OrderPaymentStatus.Canceled;
            Finish(userId);

            TEvent lastEvent = Events.Last();
            AddEvent(OrderEventType.Canceled, lastEvent.Location);
        }

        public void Return(string userId)
        {
            if (Status != OrderStatus.Delivered)
            {
                throw new InvalidOperationException(
                    "Cannot return order if it has been completed or hasn't finished delivery."
                    );
            }
            Status = OrderStatus.Returned;
            PaymentStatus = OrderPaymentStatus.Canceled;
            Finish(userId);
            AddEvent(OrderEventType.Returned, ToLocation);
        }

        public void CompletePayment()
        {
            if (PaymentStatus != OrderPaymentStatus.Due)
            {
                throw new InvalidDomainOperationException(
                    "Cannot complete payment of order if there is no due payment"
                    );
            }
            PaymentStatus = OrderPaymentStatus.Completed;
            AddEvent(OrderEventType.PaymentCompleted, ToLocation);
        }

        protected void CalculateTotals()
        {
            SubTotal = Items.Sum(i => i.TotalPrice);
            VatAmount = SubTotal * (decimal)VatRate;
            TotalAmount = SubTotal + VatAmount;
        }

        protected void AddEvent(OrderEventType type, string location, string? message = null)
        {
            var item = new TEvent
            {
                Type = type,
                Location = location,
                Message = message
            };
            Events.Add(item);
        }

        private void Finish(string userId)
        {
            FinishTime = DateTime.UtcNow;
            FinishUserId = userId;
        }
    }
}
