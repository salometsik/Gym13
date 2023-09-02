using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym13.Domain.Migrations
{
    /// <inheritdoc />
    public partial class PlanServiceIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUnlimited",
                table: "Plans");

            migrationBuilder.AddColumn<string>(
                name: "PlanServiceIds",
                table: "Plans",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanServiceIds",
                table: "Plans");

            migrationBuilder.AddColumn<bool>(
                name: "IsUnlimited",
                table: "Plans",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
