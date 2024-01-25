﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    ShippingPrice = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    PurchaseOrderId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OrderNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.PurchaseOrderId);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    UserAccountId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    UserRole = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.UserAccountId);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<string>(type: "TEXT", nullable: false),
                    Brand = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                });

            migrationBuilder.CreateTable(
                name: "Manifests",
                columns: table => new
                {
                    ManifestId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PurchaseOrderId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manifests", x => x.ManifestId);
                    table.ForeignKey(
                        name: "FK_Manifests_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    MiddleName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    Zip = table.Column<string>(type: "TEXT", nullable: false),
                    HomePhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    CellPhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PayRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    YearsWithCompany = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    CDLNumber = table.Column<string>(type: "TEXT", nullable: true),
                    CDLExperationDate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "UserAccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRecords",
                columns: table => new
                {
                    MaintenanceRecordId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Cost = table.Column<decimal>(type: "TEXT", nullable: false),
                    VehicleId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRecords", x => x.MaintenanceRecordId);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId");
                });

            migrationBuilder.CreateTable(
                name: "RepairRecords",
                columns: table => new
                {
                    RepairRecordId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Cost = table.Column<decimal>(type: "TEXT", nullable: false),
                    RepairDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VehicleId = table.Column<string>(type: "TEXT", nullable: true),
                    Cause = table.Column<string>(type: "TEXT", nullable: false),
                    Solution = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairRecords", x => x.RepairRecordId);
                    table.ForeignKey(
                        name: "FK_RepairRecords_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ManifestItems",
                columns: table => new
                {
                    ManifestItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ManifestId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateRemoved = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateReceived = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateShipped = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateReturned = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateCancelled = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateRefunded = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DatePaid = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateInvoiced = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DatePaidFor = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DatePaidInFull = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DatePaidInFullAndReceived = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DatePaidInFullAndShipped = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManifestItems", x => x.ManifestItemId);
                    table.ForeignKey(
                        name: "FK_ManifestItems_Manifests_ManifestId",
                        column: x => x.ManifestId,
                        principalTable: "Manifests",
                        principalColumn: "ManifestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ManifestItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncidentReports",
                columns: table => new
                {
                    IncidentReportId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IncidentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleId = table.Column<string>(type: "TEXT", nullable: true),
                    DriverId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsFatal = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasInjuries = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasTowedVehicle = table.Column<bool>(type: "INTEGER", nullable: false),
                    CitationIssued = table.Column<bool>(type: "INTEGER", nullable: false),
                    DrugAndAlcoholTestId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentReports", x => x.IncidentReportId);
                    table.ForeignKey(
                        name: "FK_IncidentReports_Employees_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncidentReports_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId");
                });

            migrationBuilder.CreateTable(
                name: "Payrolls",
                columns: table => new
                {
                    PayrollId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    PayPeriodStart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PayPeriodEnd = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GrossPay = table.Column<decimal>(type: "TEXT", nullable: false),
                    TaxDeductions = table.Column<decimal>(type: "TEXT", nullable: false),
                    OtherDeductions = table.Column<decimal>(type: "TEXT", nullable: false),
                    NetPay = table.Column<decimal>(type: "TEXT", nullable: false),
                    OvertimePay = table.Column<decimal>(type: "TEXT", nullable: false),
                    RegularPay = table.Column<decimal>(type: "TEXT", nullable: false),
                    SocialSecurityTax = table.Column<decimal>(type: "TEXT", nullable: false),
                    MedicareTax = table.Column<decimal>(type: "TEXT", nullable: false),
                    StateIncomeTax = table.Column<decimal>(type: "TEXT", nullable: false),
                    FederalIncomeTax = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payrolls", x => x.PayrollId);
                    table.ForeignKey(
                        name: "FK_Payrolls_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    ShipmentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Direction = table.Column<int>(type: "INTEGER", nullable: false),
                    hasArrived = table.Column<bool>(type: "INTEGER", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    Zip = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleId = table.Column<string>(type: "TEXT", nullable: false),
                    ManifestId = table.Column<int>(type: "INTEGER", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    DepDateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EstimatedArrivalTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ActualArrivalTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DriverEmployeeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.ShipmentId);
                    table.ForeignKey(
                        name: "FK_Shipments_Employees_DriverEmployeeId",
                        column: x => x.DriverEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_Shipments_Manifests_ManifestId",
                        column: x => x.ManifestId,
                        principalTable: "Manifests",
                        principalColumn: "ManifestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shipments_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shipments_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimeSheets",
                columns: table => new
                {
                    TimeSheetId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HoursWorked = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSheets", x => x.TimeSheetId);
                    table.ForeignKey(
                        name: "FK_TimeSheets_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartDetails",
                columns: table => new
                {
                    PartDetailsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartName = table.Column<string>(type: "TEXT", nullable: false),
                    PartNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Supplier = table.Column<string>(type: "TEXT", nullable: false),
                    isFromStock = table.Column<bool>(type: "INTEGER", nullable: false),
                    VehicleId = table.Column<string>(type: "TEXT", nullable: false),
                    MaintenanceRecordId = table.Column<int>(type: "INTEGER", nullable: true),
                    RepairRecordId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartDetails", x => x.PartDetailsId);
                    table.ForeignKey(
                        name: "FK_PartDetails_MaintenanceRecords_MaintenanceRecordId",
                        column: x => x.MaintenanceRecordId,
                        principalTable: "MaintenanceRecords",
                        principalColumn: "MaintenanceRecordId");
                    table.ForeignKey(
                        name: "FK_PartDetails_RepairRecords_RepairRecordId",
                        column: x => x.RepairRecordId,
                        principalTable: "RepairRecords",
                        principalColumn: "RepairRecordId");
                    table.ForeignKey(
                        name: "FK_PartDetails_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugAndAlcoholTests",
                columns: table => new
                {
                    DrugAndAlcoholTestId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DriverId = table.Column<int>(type: "INTEGER", nullable: false),
                    TestDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TestType = table.Column<int>(type: "INTEGER", nullable: false),
                    TestResult = table.Column<int>(type: "INTEGER", nullable: false),
                    TestDetails = table.Column<string>(type: "TEXT", nullable: false),
                    IncidentReportID = table.Column<int>(type: "INTEGER", nullable: true),
                    FollowUpTestDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsFollowUpComplete = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugAndAlcoholTests", x => x.DrugAndAlcoholTestId);
                    table.ForeignKey(
                        name: "FK_DrugAndAlcoholTests_Employees_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrugAndAlcoholTests_IncidentReports_IncidentReportID",
                        column: x => x.IncidentReportID,
                        principalTable: "IncidentReports",
                        principalColumn: "IncidentReportId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Details = table.Column<string>(type: "TEXT", nullable: false),
                    DriverId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipmentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_Assignments_Employees_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "ShipmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_DriverId",
                table: "Assignments",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ShipmentId",
                table: "Assignments",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugAndAlcoholTests_DriverId",
                table: "DrugAndAlcoholTests",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugAndAlcoholTests_IncidentReportID",
                table: "DrugAndAlcoholTests",
                column: "IncidentReportID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserAccountId",
                table: "Employees",
                column: "UserAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncidentReports_DriverId",
                table: "IncidentReports",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentReports_VehicleId",
                table: "IncidentReports",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_VehicleId",
                table: "MaintenanceRecords",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_ManifestItems_ManifestId",
                table: "ManifestItems",
                column: "ManifestId");

            migrationBuilder.CreateIndex(
                name: "IX_ManifestItems_ProductId",
                table: "ManifestItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Manifests_PurchaseOrderId",
                table: "Manifests",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PartDetails_MaintenanceRecordId",
                table: "PartDetails",
                column: "MaintenanceRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_PartDetails_RepairRecordId",
                table: "PartDetails",
                column: "RepairRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_PartDetails_VehicleId",
                table: "PartDetails",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_EmployeeId",
                table: "Payrolls",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairRecords_VehicleId",
                table: "RepairRecords",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_DriverEmployeeId",
                table: "Shipments",
                column: "DriverEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ManifestId",
                table: "Shipments",
                column: "ManifestId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_PurchaseOrderId",
                table: "Shipments",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_VehicleId",
                table: "Shipments",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheets_EmployeeId",
                table: "TimeSheets",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "DrugAndAlcoholTests");

            migrationBuilder.DropTable(
                name: "ManifestItems");

            migrationBuilder.DropTable(
                name: "PartDetails");

            migrationBuilder.DropTable(
                name: "Payrolls");

            migrationBuilder.DropTable(
                name: "TimeSheets");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "IncidentReports");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "MaintenanceRecords");

            migrationBuilder.DropTable(
                name: "RepairRecords");

            migrationBuilder.DropTable(
                name: "Manifests");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "UserAccounts");
        }
    }
}
