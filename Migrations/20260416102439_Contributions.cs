using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Endpoints_Salary_Calculator.Migrations
{
    /// <inheritdoc />
    public partial class Contributions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CostToCompany",
                table: "SalaryCalculations",
                type: "TEXT",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SDL",
                table: "SalaryCalculations",
                type: "TEXT",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalEmployerCosts",
                table: "SalaryCalculations",
                type: "TEXT",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UIF_Employer",
                table: "SalaryCalculations",
                type: "TEXT",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostToCompany",
                table: "SalaryCalculations");

            migrationBuilder.DropColumn(
                name: "SDL",
                table: "SalaryCalculations");

            migrationBuilder.DropColumn(
                name: "TotalEmployerCosts",
                table: "SalaryCalculations");

            migrationBuilder.DropColumn(
                name: "UIF_Employer",
                table: "SalaryCalculations");
        }
    }
}
