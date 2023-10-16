using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym13.Domain.Migrations
{
    /// <inheritdoc />
    public partial class DiscountTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Discount_DiscountId",
                table: "Plans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discount",
                table: "Discount");

            migrationBuilder.RenameTable(
                name: "Discount",
                newName: "Discounts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Discounts_DiscountId",
                table: "Plans",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "DiscountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Discounts_DiscountId",
                table: "Plans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts");

            migrationBuilder.RenameTable(
                name: "Discounts",
                newName: "Discount");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discount",
                table: "Discount",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Discount_DiscountId",
                table: "Plans",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "DiscountId");
        }
    }
}
