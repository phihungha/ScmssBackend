﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ScmssApiServer.Data;

#nullable disable

namespace ScmssApiServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ScmssApiServer.Models.PurchaseOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreateUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("FinishTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FinishUserId")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("PurchaseRequisitionId")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("SubTotal")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("VatAmount")
                        .HasColumnType("numeric");

                    b.Property<double>("VatRate")
                        .HasColumnType("double precision");

                    b.Property<int>("VendorId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CreateUserId");

                    b.HasIndex("FinishUserId");

                    b.HasIndex("PurchaseRequisitionId")
                        .IsUnique();

                    b.HasIndex("VendorId");

                    b.ToTable("PurchaseOrders");
                });

            modelBuilder.Entity("ScmssApiServer.Models.PurchaseOrderItem", b =>
                {
                    b.Property<int>("PurchaseOrderId")
                        .HasColumnType("integer");

                    b.Property<int>("SupplyId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Discount")
                        .HasColumnType("numeric");

                    b.Property<decimal>("NetPrice")
                        .HasColumnType("numeric");

                    b.Property<double>("Quantity")
                        .HasColumnType("double precision");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("numeric");

                    b.HasKey("PurchaseOrderId", "SupplyId");

                    b.HasIndex("SupplyId");

                    b.ToTable("PurchaseOrderItem");
                });

            modelBuilder.Entity("ScmssApiServer.Models.PurchaseRequisition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ApproveFinanceId")
                        .HasColumnType("text");

                    b.Property<string>("ApproveProductionManagerId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreateUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("FinishTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FinishUserId")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("SubTotal")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("VatAmount")
                        .HasColumnType("numeric");

                    b.Property<double>("VatRate")
                        .HasColumnType("double precision");

                    b.Property<int>("VendorId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ApproveFinanceId");

                    b.HasIndex("ApproveProductionManagerId");

                    b.HasIndex("CreateUserId");

                    b.HasIndex("FinishUserId");

                    b.HasIndex("VendorId");

                    b.ToTable("PurchaseRequisitions");
                });

            modelBuilder.Entity("ScmssApiServer.Models.PurchaseRequisitionItem", b =>
                {
                    b.Property<int>("PurchaseRequisitionId")
                        .HasColumnType("integer");

                    b.Property<int>("SupplyId")
                        .HasColumnType("integer");

                    b.Property<double>("Quantity")
                        .HasColumnType("double precision");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("numeric");

                    b.HasKey("PurchaseRequisitionId", "SupplyId");

                    b.HasIndex("SupplyId");

                    b.ToTable("PurchaseRequisitionItem");
                });

            modelBuilder.Entity("ScmssApiServer.Models.Supply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("VendorId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("VendorId");

                    b.ToTable("Supplies");
                });

            modelBuilder.Entity("ScmssApiServer.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IdCardNumber")
                        .HasMaxLength(12)
                        .HasColumnType("char(12)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("ScmssApiServer.Models.Vendor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ContactPerson")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ScmssApiServer.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ScmssApiServer.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScmssApiServer.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ScmssApiServer.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScmssApiServer.Models.PurchaseOrder", b =>
                {
                    b.HasOne("ScmssApiServer.Models.User", "CreateUser")
                        .WithMany("CreatedPurchaseOrders")
                        .HasForeignKey("CreateUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScmssApiServer.Models.User", "FinishUser")
                        .WithMany("FinishedPurchaseOrders")
                        .HasForeignKey("FinishUserId");

                    b.HasOne("ScmssApiServer.Models.PurchaseRequisition", "PurchaseRequisition")
                        .WithOne("PurchaseOrder")
                        .HasForeignKey("ScmssApiServer.Models.PurchaseOrder", "PurchaseRequisitionId");

                    b.HasOne("ScmssApiServer.Models.Vendor", "Vendor")
                        .WithMany("PurchaseOrders")
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreateUser");

                    b.Navigation("FinishUser");

                    b.Navigation("PurchaseRequisition");

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("ScmssApiServer.Models.PurchaseOrderItem", b =>
                {
                    b.HasOne("ScmssApiServer.Models.PurchaseOrder", "PurchaseOrder")
                        .WithMany("PurchaseOrderItems")
                        .HasForeignKey("PurchaseOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScmssApiServer.Models.Supply", "Supply")
                        .WithMany("PurchaseOrderItems")
                        .HasForeignKey("SupplyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PurchaseOrder");

                    b.Navigation("Supply");
                });

            modelBuilder.Entity("ScmssApiServer.Models.PurchaseRequisition", b =>
                {
                    b.HasOne("ScmssApiServer.Models.User", "ApproveFinance")
                        .WithMany("ApprovedPurchaseRequisitionsAsFinance")
                        .HasForeignKey("ApproveFinanceId");

                    b.HasOne("ScmssApiServer.Models.User", "ApproveProductionManager")
                        .WithMany("ApprovedPurchaseRequisitionsAsManager")
                        .HasForeignKey("ApproveProductionManagerId");

                    b.HasOne("ScmssApiServer.Models.User", "CreateUser")
                        .WithMany("CreatedPurchaseRequisitions")
                        .HasForeignKey("CreateUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScmssApiServer.Models.User", "FinishUser")
                        .WithMany("FinishedPurchaseRequisitions")
                        .HasForeignKey("FinishUserId");

                    b.HasOne("ScmssApiServer.Models.Vendor", "Vendor")
                        .WithMany("PurchaseRequisitions")
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApproveFinance");

                    b.Navigation("ApproveProductionManager");

                    b.Navigation("CreateUser");

                    b.Navigation("FinishUser");

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("ScmssApiServer.Models.PurchaseRequisitionItem", b =>
                {
                    b.HasOne("ScmssApiServer.Models.PurchaseRequisition", "PurchaseRequisition")
                        .WithMany("PurchaseRequisitionItems")
                        .HasForeignKey("PurchaseRequisitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScmssApiServer.Models.Supply", "Supply")
                        .WithMany("PurchaseRequisitionItems")
                        .HasForeignKey("SupplyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PurchaseRequisition");

                    b.Navigation("Supply");
                });

            modelBuilder.Entity("ScmssApiServer.Models.Supply", b =>
                {
                    b.HasOne("ScmssApiServer.Models.Vendor", "Vendor")
                        .WithMany("Supplies")
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("ScmssApiServer.Models.PurchaseOrder", b =>
                {
                    b.Navigation("PurchaseOrderItems");
                });

            modelBuilder.Entity("ScmssApiServer.Models.PurchaseRequisition", b =>
                {
                    b.Navigation("PurchaseOrder");

                    b.Navigation("PurchaseRequisitionItems");
                });

            modelBuilder.Entity("ScmssApiServer.Models.Supply", b =>
                {
                    b.Navigation("PurchaseOrderItems");

                    b.Navigation("PurchaseRequisitionItems");
                });

            modelBuilder.Entity("ScmssApiServer.Models.User", b =>
                {
                    b.Navigation("ApprovedPurchaseRequisitionsAsFinance");

                    b.Navigation("ApprovedPurchaseRequisitionsAsManager");

                    b.Navigation("CreatedPurchaseOrders");

                    b.Navigation("CreatedPurchaseRequisitions");

                    b.Navigation("FinishedPurchaseOrders");

                    b.Navigation("FinishedPurchaseRequisitions");
                });

            modelBuilder.Entity("ScmssApiServer.Models.Vendor", b =>
                {
                    b.Navigation("PurchaseOrders");

                    b.Navigation("PurchaseRequisitions");

                    b.Navigation("Supplies");
                });
#pragma warning restore 612, 618
        }
    }
}
