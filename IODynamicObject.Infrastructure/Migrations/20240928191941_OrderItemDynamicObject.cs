using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IODynamicObject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderItemDynamicObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Objects_ObjectId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Values_Fields_FieldId",
                table: "Values");

            migrationBuilder.DropIndex(
                name: "IX_Values_FieldId",
                table: "Values");

            migrationBuilder.DropIndex(
                name: "IX_Fields_ObjectId",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Objects");

            migrationBuilder.AddColumn<long>(
                name: "IOFieldId",
                table: "Values",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IOOrderItemId",
                table: "Objects",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IOObjectId",
                table: "Fields",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Values_IOFieldId",
                table: "Values",
                column: "IOFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_IOOrderItemId",
                table: "Objects",
                column: "IOOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_IOObjectId",
                table: "Fields",
                column: "IOObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Objects_IOObjectId",
                table: "Fields",
                column: "IOObjectId",
                principalTable: "Objects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_OrderItems_IOOrderItemId",
                table: "Objects",
                column: "IOOrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Values_Fields_IOFieldId",
                table: "Values",
                column: "IOFieldId",
                principalTable: "Fields",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Objects_IOObjectId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Objects_OrderItems_IOOrderItemId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_Values_Fields_IOFieldId",
                table: "Values");

            migrationBuilder.DropIndex(
                name: "IX_Values_IOFieldId",
                table: "Values");

            migrationBuilder.DropIndex(
                name: "IX_Objects_IOOrderItemId",
                table: "Objects");

            migrationBuilder.DropIndex(
                name: "IX_Fields_IOObjectId",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "IOFieldId",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "IOOrderItemId",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "IOObjectId",
                table: "Fields");

            migrationBuilder.AddColumn<long>(
                name: "EntityId",
                table: "Objects",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Values_FieldId",
                table: "Values",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_ObjectId",
                table: "Fields",
                column: "ObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Objects_ObjectId",
                table: "Fields",
                column: "ObjectId",
                principalTable: "Objects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Values_Fields_FieldId",
                table: "Values",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
