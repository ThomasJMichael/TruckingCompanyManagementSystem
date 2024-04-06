using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixOnModelCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Employees_DriverId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_DrugAndAlcoholTests_Employees_DriverId",
                table: "DrugAndAlcoholTests");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidentReports_Employees_DriverId",
                table: "IncidentReports");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                table: "IncidentReports",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_IncidentReports_DriverId",
                table: "IncidentReports",
                newName: "IX_IncidentReports_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                table: "DrugAndAlcoholTests",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_DrugAndAlcoholTests_DriverId",
                table: "DrugAndAlcoholTests",
                newName: "IX_DrugAndAlcoholTests_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                table: "Assignments",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_DriverId",
                table: "Assignments",
                newName: "IX_Assignments_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Employees_EmployeeId",
                table: "Assignments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DrugAndAlcoholTests_Employees_EmployeeId",
                table: "DrugAndAlcoholTests",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentReports_Employees_EmployeeId",
                table: "IncidentReports",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Employees_EmployeeId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_DrugAndAlcoholTests_Employees_EmployeeId",
                table: "DrugAndAlcoholTests");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidentReports_Employees_EmployeeId",
                table: "IncidentReports");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "IncidentReports",
                newName: "DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_IncidentReports_EmployeeId",
                table: "IncidentReports",
                newName: "IX_IncidentReports_DriverId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "DrugAndAlcoholTests",
                newName: "DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_DrugAndAlcoholTests_EmployeeId",
                table: "DrugAndAlcoholTests",
                newName: "IX_DrugAndAlcoholTests_DriverId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Assignments",
                newName: "DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_EmployeeId",
                table: "Assignments",
                newName: "IX_Assignments_DriverId");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Employees",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Employees_DriverId",
                table: "Assignments",
                column: "DriverId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DrugAndAlcoholTests_Employees_DriverId",
                table: "DrugAndAlcoholTests",
                column: "DriverId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentReports_Employees_DriverId",
                table: "IncidentReports",
                column: "DriverId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
