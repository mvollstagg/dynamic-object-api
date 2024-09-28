using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IODynamicObject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DynamicObjectBecomaTrackableEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Values",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateUtc",
                table: "Values",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ModificatedById",
                table: "Values",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDateUtc",
                table: "Values",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Objects",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateUtc",
                table: "Objects",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ModificatedById",
                table: "Objects",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDateUtc",
                table: "Objects",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Fields",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateUtc",
                table: "Fields",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ModificatedById",
                table: "Fields",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDateUtc",
                table: "Fields",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "CreationDateUtc",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "ModificatedById",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "ModificationDateUtc",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "CreationDateUtc",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "ModificatedById",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "ModificationDateUtc",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "CreationDateUtc",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "ModificatedById",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "ModificationDateUtc",
                table: "Fields");
        }
    }
}
