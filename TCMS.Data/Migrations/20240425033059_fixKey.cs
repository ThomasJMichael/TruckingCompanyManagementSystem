using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Manifests_PurchaseOrders_PurchaseOrderId",
                table: "Manifests");

            migrationBuilder.DropForeignKey(
                name: "FK_Manifests_Shipments_ShipmentId",
                table: "Manifests");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_PurchaseOrders_ManifestId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_ManifestId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Manifests_PurchaseOrderId",
                table: "Manifests");

            migrationBuilder.DropIndex(
                name: "IX_Manifests_ShipmentId",
                table: "Manifests");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                table: "Manifests");

            migrationBuilder.DropColumn(
                name: "ShipmentId",
                table: "Manifests");

            migrationBuilder.AlterColumn<int>(
                name: "ManifestId",
                table: "Shipments",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "ManifestId",
                table: "PurchaseOrders",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ManifestId",
                table: "Shipments",
                column: "ManifestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_PurchaseOrderId",
                table: "Shipments",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ManifestId",
                table: "PurchaseOrders",
                column: "ManifestId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Manifests_ManifestId",
                table: "PurchaseOrders",
                column: "ManifestId",
                principalTable: "Manifests",
                principalColumn: "ManifestId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Manifests_ManifestId",
                table: "Shipments",
                column: "ManifestId",
                principalTable: "Manifests",
                principalColumn: "ManifestId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_PurchaseOrders_PurchaseOrderId",
                table: "Shipments",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "PurchaseOrderId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Manifests_ManifestId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Manifests_ManifestId",
                table: "Shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_PurchaseOrders_PurchaseOrderId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_ManifestId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_PurchaseOrderId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_ManifestId",
                table: "PurchaseOrders");

            migrationBuilder.AlterColumn<int>(
                name: "ManifestId",
                table: "Shipments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ManifestId",
                table: "PurchaseOrders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderId",
                table: "Manifests",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShipmentId",
                table: "Manifests",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ManifestId",
                table: "Shipments",
                column: "ManifestId");

            migrationBuilder.CreateIndex(
                name: "IX_Manifests_PurchaseOrderId",
                table: "Manifests",
                column: "PurchaseOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manifests_ShipmentId",
                table: "Manifests",
                column: "ShipmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Manifests_PurchaseOrders_PurchaseOrderId",
                table: "Manifests",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "PurchaseOrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Manifests_Shipments_ShipmentId",
                table: "Manifests",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "ShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_PurchaseOrders_ManifestId",
                table: "Shipments",
                column: "ManifestId",
                principalTable: "PurchaseOrders",
                principalColumn: "PurchaseOrderId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
