using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IODynamicObject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DynamicObjectUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObjectType",
                table: "IODynamicObjects");

            migrationBuilder.AddColumn<byte>(
                name: "SchemaType",
                table: "IODynamicObjects",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchemaType",
                table: "IODynamicObjects");

            migrationBuilder.AddColumn<string>(
                name: "ObjectType",
                table: "IODynamicObjects",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
