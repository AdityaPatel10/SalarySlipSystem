using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalarySlipManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class AddOtpFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OtpEpiry",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RestOtp",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtpEpiry",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RestOtp",
                table: "Employees");
        }
    }
}
