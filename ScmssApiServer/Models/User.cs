﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ScmssApiServer.DTOs;
using ScmssApiServer.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class User : IdentityUser, ISoftDeletable
    {
        public static readonly string ImageFolderKey = "user-profile-images";

        [PersonalData]
        public string? Address { get; set; }

        public ICollection<ProductionOrder> ApprovedProductionOrdersAsManager { get; set; }
            = new List<ProductionOrder>();

        public ICollection<PurchaseRequisition> ApprovedPurchaseRequisitionsAsFinance { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<PurchaseRequisition> ApprovedPurchaseRequisitionsAsManager { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<ProductionOrder> CreatedProductionOrders { get; set; }
            = new List<ProductionOrder>();

        public ICollection<PurchaseOrder> CreatedPurchaseOrders { get; set; }
            = new List<PurchaseOrder>();

        public ICollection<PurchaseRequisition> CreatedPurchaseRequisitions { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<SalesOrder> CreatedSalesOrders { get; set; }
            = new List<SalesOrder>();

        public DateTime CreateTime { get; set; }

        [PersonalData]
        public required DateTime DateOfBirth { get; set; }

        public string? Description { get; set; }

        public ICollection<ProductionOrder> FinishedProductionOrders { get; set; }
            = new List<ProductionOrder>();

        public ICollection<PurchaseOrder> FinishedPurchaseOrders { get; set; }
            = new List<PurchaseOrder>();

        public ICollection<PurchaseRequisition> FinishedPurchaseRequisitions { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<SalesOrder> FinishedSalesOrders { get; set; }
            = new List<SalesOrder>();

        [PersonalData]
        public required Gender Gender { get; set; }

        [PersonalData]
        [StringLength(maximumLength: 12, MinimumLength = 12)]
        [Column(TypeName = "char(12)")]
        public string? IdCardNumber { get; set; }

        public string? ImageName { get; set; }

        [NotMapped]
        public Uri? ImageUrl => ImageName != null ? FileHostService.GetUri(ImageFolderKey, ImageName) : null;

        public bool IsActive { get; set; }

        [PersonalData]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public required string Name { get; set; }

        public ProductionFacility? ProductionFacility { get; set; }

        public int? ProductionFacilityId { get; set; }
        public DateTime? UpdateTime { get; set; }
    }

    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>().ForMember(
                i => i.DateOfBirth,
                o => o.MapFrom(i => i.DateOfBirth.ToString(
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture)));

            CreateMap<UserInputDto, User>().Include<UserCreateDto, User>()
                .ForMember(i => i.ProductionFacilityId, o => o.AllowNull())
                .ForMember(
                    i => i.DateOfBirth,
                    o => o.MapFrom(
                        i => DateOnly.Parse(i.DateOfBirth)
                                     .ToDateTime(new TimeOnly(), DateTimeKind.Utc)));

            CreateMap<UserCreateDto, User>();
        }
    }
}
