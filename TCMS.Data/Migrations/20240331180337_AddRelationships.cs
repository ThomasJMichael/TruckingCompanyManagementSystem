using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShippingCostPaid",
                table: "Shipments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                table: "Shipments",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingCostPaymentDate",
                table: "Shipments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "ManifestItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RecordType",
                table: "MaintenanceRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShippingCostPaid",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ShippingCostPaymentDate",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "ManifestItems");

            migrationBuilder.DropColumn(
                name: "RecordType",
                table: "MaintenanceRecords");
        }
    }
}
