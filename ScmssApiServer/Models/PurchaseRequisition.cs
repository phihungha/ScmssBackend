﻿using AutoMapper;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents a purchase requisition.
    /// </summary>
    public class PurchaseRequisition : StandardLifecycle
    {
        public ApprovalStatus ApprovalStatus { get; protected set; }
        public User? ApproveFinance { get; protected set; }
        public string? ApproveFinanceId { get; protected set; }
        public User? ApproveProductionManager { get; protected set; }
        public string? ApproveProductionManagerId { get; protected set; }
        public int Id { get; set; }
        public bool IsApprovalAllowed => !IsEnded && ApprovalStatus == ApprovalStatus.PendingApproval;
        public bool IsCancelAllowed => !IsEnded;
        public override bool IsCreated => Id != 0;

        public bool IsInfoUpdateAllowed => !IsEnded && ApprovalStatus == ApprovalStatus.PendingApproval;

        public bool IsPurchaseOrderCreateAllowed =>
                    !IsEnded &&
            ApprovalStatus == ApprovalStatus.Approved &&
            Status != PurchaseRequisitionStatus.Purchasing;

        public ICollection<PurchaseRequisitionItem> Items { get; protected set; }
                            = new List<PurchaseRequisitionItem>();

        public ProductionFacility ProductionFacility { get; set; } = null!;

        public int ProductionFacilityId { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; protected set; }
            = new List<PurchaseOrder>();

        public PurchaseRequisitionStatus Status { get; protected set; }
        public decimal SubTotal { get; private set; }
        public ICollection<Supply> Supplies { get; protected set; } = new List<Supply>();
        public decimal TotalAmount { get; protected set; }
        public decimal VatAmount { get; protected set; }
        public double VatRate { get; set; }
        public Vendor Vendor { get; set; } = null!;
        public int VendorId { get; set; }

        public void AddItems(ICollection<PurchaseRequisitionItem> items)
        {
            if (IsEnded || ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot add items after the requisition has ended or approved/rejected."
                );
            }

            int duplicateCount = items.GroupBy(x => x.ItemId).Count(g => g.Count() > 1);
            if (duplicateCount > 0)
            {
                throw new InvalidDomainOperationException("Duplicate requisition item IDs found.");
            }

            Items = items;
            CalculateTotals();
        }

        public void ApproveAsFinance(User user)
        {
            ApproveFinanceId = user.Id;
            ApproveFinance = user;
            Approve();
        }

        public void ApproveAsProductionManager(User user)
        {
            ApproveProductionManagerId = user.Id;
            ApproveProductionManager = user;
            Approve();
        }

        public override void Begin(User user)
        {
            base.Begin(user);
            Status = PurchaseRequisitionStatus.Processing;
            ApprovalStatus = ApprovalStatus.PendingApproval;
        }

        public void Cancel(User user, string problem)
        {
            if (IsEnded)
            {
                throw new InvalidDomainOperationException(
                        "Cannot cancel an ended purchase requisition."
                    );
            }
            Status = PurchaseRequisitionStatus.Canceled;
            EndWithProblem(user, problem);
        }

        public void Complete(User user)
        {
            if (Status != PurchaseRequisitionStatus.Purchasing)
            {
                throw new InvalidDomainOperationException(
                        "Cannot complete a purchase requisition which isn't executing."
                    );
            }
            Status = PurchaseRequisitionStatus.Completed;
            End(user);
        }

        public void Delay(string problem)
        {
            if (Status != PurchaseRequisitionStatus.Purchasing)
            {
                throw new InvalidDomainOperationException(
                        "Cannot delay a purchase requisition which isn't executing."
                    );
            }
            Status = PurchaseRequisitionStatus.Delayed;
            Problem = problem;
        }

        public PurchaseOrder GeneratePurchaseOrder(string userId)
        {
            if (ApprovalStatus != ApprovalStatus.Approved)
            {
                throw new InvalidDomainOperationException(
                        "Cannot create purchase order from an unapproved requisition."
                    );
            }

            if (IsEnded)
            {
                throw new InvalidDomainOperationException(
                        "Cannot create purchase order from an ended requisition."
                    );
            }

            if (Status == PurchaseRequisitionStatus.Purchasing)
            {
                throw new InvalidDomainOperationException(
                        "There is already a purchase order ongoing for this requisition."
                    );
            }

            var order = new PurchaseOrder
            {
                PurchaseRequisitionId = Id,
                PurchaseRequisition = this,
                VendorId = VendorId,
                Vendor = Vendor,
                ProductionFacilityId = ProductionFacilityId,
                ProductionFacility = ProductionFacility,
                CreateUserId = userId,
                FromLocation = Vendor.DefaultLocation,
                ToLocation = ProductionFacility.Location,
            };

            var orderItems = Items.Select(i => new PurchaseOrderItem
            {
                ItemId = i.ItemId,
                Quantity = i.Quantity,
                Unit = i.Unit,
                UnitPrice = i.UnitPrice,
            }).ToList();
            order.AddItems(orderItems);

            PurchaseOrders.Add(order);
            Status = PurchaseRequisitionStatus.Purchasing;

            return order;
        }

        public virtual void Reject(User user, string problem)
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot reject a purchase requisition which isn't waiting approval."
                    );
            }
            ApprovalStatus = ApprovalStatus.Rejected;
            Cancel(user, problem);
        }

        protected virtual void Approve()
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot approve a purchase requisition which isn't waiting approval."
                    );
            }

            if (ApproveProductionManager != null && ApproveFinance != null)
            {
                ApprovalStatus = ApprovalStatus.Approved;
            }
        }

        protected void CalculateTotals()
        {
            SubTotal = Items.Sum(i => i.TotalPrice);
            VatAmount = SubTotal * (decimal)VatRate;
            TotalAmount = SubTotal + VatAmount;
        }
    }

    public class PurchaseRequisitionMp : Profile
    {
        public PurchaseRequisitionMp()
        {
            CreateMap<PurchaseRequisition, PurchaseRequisitionDto>();
        }
    }
}
