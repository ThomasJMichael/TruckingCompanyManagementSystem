﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TCMS.Data.Data;

#nullable disable

namespace TCMS.Data.Migrations
{
    [DbContext(typeof(TcmsContext))]
    partial class TcmsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

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
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TCMS.Data.Models.Assignment", b =>
                {
                    b.Property<int>("AssignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DriverId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("ShipmentId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("AssignmentId");

                    b.HasIndex("DriverId");

                    b.HasIndex("ShipmentId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("TCMS.Data.Models.DrugAndAlcoholTest", b =>
                {
                    b.Property<int>("DrugAndAlcoholTestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DriverId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("FollowUpTestDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IncidentReportId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsFollowUpComplete")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TestDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("TestDetails")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TestResult")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TestType")
                        .HasColumnType("INTEGER");

                    b.HasKey("DrugAndAlcoholTestId");

                    b.HasIndex("DriverId");

                    b.HasIndex("IncidentReportId")
                        .IsUnique();

                    b.ToTable("DrugAndAlcoholTests");
                });

            modelBuilder.Entity("TCMS.Data.Models.Employee", b =>
                {
                    b.Property<string>("EmployeeId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CellPhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("HomePhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PayRate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserAccountId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Zip")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EmployeeId");

                    b.HasIndex("UserAccountId")
                        .IsUnique();

                    b.ToTable("Employees");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Employee");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("TCMS.Data.Models.IncidentReport", b =>
                {
                    b.Property<int>("IncidentReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CitationIssued")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DriverId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("DrugAndAlcoholTestId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasInjuries")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasTowedVehicle")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("IncidentDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsFatal")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VehicleId")
                        .HasColumnType("TEXT");

                    b.HasKey("IncidentReportId");

                    b.HasIndex("DriverId");

                    b.HasIndex("VehicleId");

                    b.ToTable("IncidentReports");
                });

            modelBuilder.Entity("TCMS.Data.Models.MaintenanceRecord", b =>
                {
                    b.Property<int>("MaintenanceRecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Cost")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("MaintenanceDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("RecordType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VehicleId")
                        .HasColumnType("TEXT");

                    b.HasKey("MaintenanceRecordId");

                    b.HasIndex("VehicleId");

                    b.ToTable("MaintenanceRecords");
                });

            modelBuilder.Entity("TCMS.Data.Models.Manifest", b =>
                {
                    b.Property<int>("ManifestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PurchaseOrderId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ManifestId");

                    b.HasIndex("PurchaseOrderId");

                    b.ToTable("Manifests");
                });

            modelBuilder.Entity("TCMS.Data.Models.ManifestItem", b =>
                {
                    b.Property<int>("ManifestItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateCancelled")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateInvoiced")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DatePaid")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DatePaidFor")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DatePaidInFull")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DatePaidInFullAndReceived")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DatePaidInFullAndShipped")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateReceived")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateRefunded")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateRemoved")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateReturned")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateShipped")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ManifestId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("ManifestItemId");

                    b.HasIndex("ManifestId");

                    b.HasIndex("ProductId");

                    b.ToTable("ManifestItems");
                });

            modelBuilder.Entity("TCMS.Data.Models.PartDetails", b =>
                {
                    b.Property<int>("PartDetailsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MaintenanceRecordId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PartName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PartNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RepairRecordId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Supplier")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("VehicleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("isFromStock")
                        .HasColumnType("INTEGER");

                    b.HasKey("PartDetailsId");

                    b.HasIndex("MaintenanceRecordId");

                    b.HasIndex("RepairRecordId");

                    b.HasIndex("VehicleId");

                    b.ToTable("PartDetails");
                });

            modelBuilder.Entity("TCMS.Data.Models.Payroll", b =>
                {
                    b.Property<int>("PayrollId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmployeeId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("FederalIncomeTax")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("GrossPay")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("MedicareTax")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("NetPay")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OtherDeductions")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OvertimePay")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PayPeriodEnd")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PayPeriodStart")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("RegularPay")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SocialSecurityTax")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("StateIncomeTax")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TaxDeductions")
                        .HasColumnType("TEXT");

                    b.HasKey("PayrollId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Payrolls");
                });

            modelBuilder.Entity("TCMS.Data.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ShippingPrice")
                        .HasColumnType("TEXT");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("TCMS.Data.Models.PurchaseOrder", b =>
                {
                    b.Property<int>("PurchaseOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PurchaseOrderId");

                    b.ToTable("PurchaseOrders");
                });

            modelBuilder.Entity("TCMS.Data.Models.RepairRecord", b =>
                {
                    b.Property<int>("RepairRecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cause")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Cost")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RepairDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Solution")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("VehicleId")
                        .HasColumnType("TEXT");

                    b.HasKey("RepairRecordId");

                    b.HasIndex("VehicleId");

                    b.ToTable("RepairRecords");
                });

            modelBuilder.Entity("TCMS.Data.Models.Shipment", b =>
                {
                    b.Property<int>("ShipmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ActualArrivalTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DepDateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Direction")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DriverEmployeeId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EstimatedArrivalTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsShippingCostPaid")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ManifestId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PurchaseOrderId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ShippingCost")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ShippingCostPaymentDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("VehicleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Zip")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("hasArrived")
                        .HasColumnType("INTEGER");

                    b.HasKey("ShipmentId");

                    b.HasIndex("DriverEmployeeId");

                    b.HasIndex("ManifestId");

                    b.HasIndex("PurchaseOrderId");

                    b.HasIndex("VehicleId");

                    b.ToTable("Shipments");
                });

            modelBuilder.Entity("TCMS.Data.Models.TimeSheet", b =>
                {
                    b.Property<int>("TimeSheetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ClockIn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ClockOut")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmployeeId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TimeSheetId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("TimeSheets");
                });

            modelBuilder.Entity("TCMS.Data.Models.UserAccount", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("TCMS.Data.Models.Vehicle", b =>
                {
                    b.Property<string>("VehicleId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("VehicleId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("TCMS.Data.Models.Driver", b =>
                {
                    b.HasBaseType("TCMS.Data.Models.Employee");

                    b.Property<DateTime>("CDLExperationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CDLNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("Driver");
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
                    b.HasOne("TCMS.Data.Models.UserAccount", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TCMS.Data.Models.UserAccount", null)
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

                    b.HasOne("TCMS.Data.Models.UserAccount", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TCMS.Data.Models.UserAccount", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TCMS.Data.Models.Assignment", b =>
                {
                    b.HasOne("TCMS.Data.Models.Driver", "Driver")
                        .WithMany("Assignments")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TCMS.Data.Models.Shipment", "Shipment")
                        .WithMany("Assignments")
                        .HasForeignKey("ShipmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Driver");

                    b.Navigation("Shipment");
                });

            modelBuilder.Entity("TCMS.Data.Models.DrugAndAlcoholTest", b =>
                {
                    b.HasOne("TCMS.Data.Models.Driver", "Driver")
                        .WithMany("DrugAndAlcoholTests")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TCMS.Data.Models.IncidentReport", "IncidentReport")
                        .WithOne("DrugAndAlcoholTest")
                        .HasForeignKey("TCMS.Data.Models.DrugAndAlcoholTest", "IncidentReportId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Driver");

                    b.Navigation("IncidentReport");
                });

            modelBuilder.Entity("TCMS.Data.Models.Employee", b =>
                {
                    b.HasOne("TCMS.Data.Models.UserAccount", "UserAccount")
                        .WithOne("Employee")
                        .HasForeignKey("TCMS.Data.Models.Employee", "UserAccountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("UserAccount");
                });

            modelBuilder.Entity("TCMS.Data.Models.IncidentReport", b =>
                {
                    b.HasOne("TCMS.Data.Models.Driver", "Driver")
                        .WithMany("IncidentReports")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TCMS.Data.Models.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId");

                    b.Navigation("Driver");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("TCMS.Data.Models.MaintenanceRecord", b =>
                {
                    b.HasOne("TCMS.Data.Models.Vehicle", "Vehicle")
                        .WithMany("MaintenanceRecords")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("TCMS.Data.Models.Manifest", b =>
                {
                    b.HasOne("TCMS.Data.Models.PurchaseOrder", "PurchaseOrder")
                        .WithMany("Manifests")
                        .HasForeignKey("PurchaseOrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("PurchaseOrder");
                });

            modelBuilder.Entity("TCMS.Data.Models.ManifestItem", b =>
                {
                    b.HasOne("TCMS.Data.Models.Manifest", "Manifest")
                        .WithMany("ManifestItems")
                        .HasForeignKey("ManifestId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TCMS.Data.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Manifest");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("TCMS.Data.Models.PartDetails", b =>
                {
                    b.HasOne("TCMS.Data.Models.MaintenanceRecord", "MaintenanceRecord")
                        .WithMany("PartDetails")
                        .HasForeignKey("MaintenanceRecordId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TCMS.Data.Models.RepairRecord", "RepairRecord")
                        .WithMany()
                        .HasForeignKey("RepairRecordId");

                    b.HasOne("TCMS.Data.Models.Vehicle", "Vehicle")
                        .WithMany("Parts")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("MaintenanceRecord");

                    b.Navigation("RepairRecord");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("TCMS.Data.Models.Payroll", b =>
                {
                    b.HasOne("TCMS.Data.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("TCMS.Data.Models.RepairRecord", b =>
                {
                    b.HasOne("TCMS.Data.Models.Vehicle", "Vehicle")
                        .WithMany("RepairRecords")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("TCMS.Data.Models.Shipment", b =>
                {
                    b.HasOne("TCMS.Data.Models.Driver", null)
                        .WithMany("Shipments")
                        .HasForeignKey("DriverEmployeeId");

                    b.HasOne("TCMS.Data.Models.Manifest", "Manifest")
                        .WithMany()
                        .HasForeignKey("ManifestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TCMS.Data.Models.PurchaseOrder", "PurchaseOrder")
                        .WithMany()
                        .HasForeignKey("PurchaseOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TCMS.Data.Models.Vehicle", "Vehicle")
                        .WithMany("Shipments")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Manifest");

                    b.Navigation("PurchaseOrder");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("TCMS.Data.Models.TimeSheet", b =>
                {
                    b.HasOne("TCMS.Data.Models.Employee", "Employee")
                        .WithMany("TimeSheets")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("TCMS.Data.Models.Employee", b =>
                {
                    b.Navigation("TimeSheets");
                });

            modelBuilder.Entity("TCMS.Data.Models.IncidentReport", b =>
                {
                    b.Navigation("DrugAndAlcoholTest")
                        .IsRequired();
                });

            modelBuilder.Entity("TCMS.Data.Models.MaintenanceRecord", b =>
                {
                    b.Navigation("PartDetails");
                });

            modelBuilder.Entity("TCMS.Data.Models.Manifest", b =>
                {
                    b.Navigation("ManifestItems");
                });

            modelBuilder.Entity("TCMS.Data.Models.PurchaseOrder", b =>
                {
                    b.Navigation("Manifests");
                });

            modelBuilder.Entity("TCMS.Data.Models.Shipment", b =>
                {
                    b.Navigation("Assignments");
                });

            modelBuilder.Entity("TCMS.Data.Models.UserAccount", b =>
                {
                    b.Navigation("Employee")
                        .IsRequired();
                });

            modelBuilder.Entity("TCMS.Data.Models.Vehicle", b =>
                {
                    b.Navigation("MaintenanceRecords");

                    b.Navigation("Parts");

                    b.Navigation("RepairRecords");

                    b.Navigation("Shipments");
                });

            modelBuilder.Entity("TCMS.Data.Models.Driver", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("DrugAndAlcoholTests");

                    b.Navigation("IncidentReports");

                    b.Navigation("Shipments");
                });
#pragma warning restore 612, 618
        }
    }
}
