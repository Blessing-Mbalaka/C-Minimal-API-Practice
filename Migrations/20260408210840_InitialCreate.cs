using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Endpoints_Salary_Calculator.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    JobTitle = table.Column<string>(type: "TEXT", nullable: false),
                    EmploymentType = table.Column<string>(type: "TEXT", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalaryCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Period = table.Column<string>(type: "TEXT", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    RegularHours = table.Column<int>(type: "INTEGER", nullable: false),
                    OvertimeHours = table.Column<int>(type: "INTEGER", nullable: false),
                    VariableAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    BasicSalary = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    OvertimePay = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    GrossSalary = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    UIF = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    PAYE = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false), // added retirement fund deduction
                    RetirementFund = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    TotalDeductions = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    NetSalary = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryCalculations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryCalculations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalaryCalculations_EmployeeId",
                table: "SalaryCalculations",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalaryCalculations");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
