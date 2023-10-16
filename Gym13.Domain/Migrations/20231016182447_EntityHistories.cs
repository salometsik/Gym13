using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gym13.Domain.Migrations
{
    /// <inheritdoc />
    public partial class EntityHistories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalNumber",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Trainers",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Plans",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<string>(
                name: "IdentificationNumber",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Trainers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PlanServices",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PlanServices",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Plans",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "InfoTabs",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "InfoTabs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Discounts",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Banners",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Banners",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EntityHistories",
                columns: table => new
                {
                    EntityHistoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Table = table.Column<string>(type: "text", nullable: false),
                    Column = table.Column<string>(type: "text", nullable: false),
                    ActionType = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityHistories", x => x.EntityHistoryId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityHistories");

            migrationBuilder.DropColumn(
                name: "IdentificationNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PlanServices");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PlanServices");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "InfoTabs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "InfoTabs");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Banners");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Trainers",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Plans",
                newName: "CreateDate");

            migrationBuilder.AddColumn<string>(
                name: "PersonalNumber",
                table: "Users",
                type: "text",
                nullable: true);
        }
    }
}
