using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IODynamicObject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OnModelCreatingChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Customers_EntityId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Order_EntityId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Products_EntityId",
                table: "Objects");

            migrationBuilder.DropIndex(
                name: "IX_Objects_EntityId",
                table: "Objects");

            migrationBuilder.AddColumn<long>(
                name: "IOCustomerId",
                table: "Objects",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IOOrderId",
                table: "Objects",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IOProductId",
                table: "Objects",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objects_IOCustomerId",
                table: "Objects",
                column: "IOCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_IOOrderId",
                table: "Objects",
                column: "IOOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_IOProductId",
                table: "Objects",
                column: "IOProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Customers_IOCustomerId",
                table: "Objects",
                column: "IOCustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Order_IOOrderId",
                table: "Objects",
                column: "IOOrderId",
                principalTable: "Order",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Products_IOProductId",
                table: "Objects",
                column: "IOProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Customers_IOCustomerId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Order_IOOrderId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Products_IOProductId",
                table: "Objects");

            migrationBuilder.DropIndex(
                name: "IX_Objects_IOCustomerId",
                table: "Objects");

            migrationBuilder.DropIndex(
                name: "IX_Objects_IOOrderId",
                table: "Objects");

            migrationBuilder.DropIndex(
                name: "IX_Objects_IOProductId",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "IOCustomerId",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "IOOrderId",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "IOProductId",
                table: "Objects");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_EntityId",
                table: "Objects",
                column: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Customers_EntityId",
                table: "Objects",
                column: "EntityId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Order_EntityId",
                table: "Objects",
                column: "EntityId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Products_EntityId",
                table: "Objects",
                column: "EntityId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
