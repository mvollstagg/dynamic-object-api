using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IODynamicObject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderEntityUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_OrderItems_IOOrderItemId",
                table: "Objects");

            migrationBuilder.DropIndex(
                name: "IX_Objects_IOOrderItemId",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IOOrderItemId",
                table: "Objects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Order",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "IOOrderItemId",
                table: "Objects",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objects_IOOrderItemId",
                table: "Objects",
                column: "IOOrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_OrderItems_IOOrderItemId",
                table: "Objects",
                column: "IOOrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id");
        }
    }
}
