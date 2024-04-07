using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace multiple_tables.Migrations
{
    /// <inheritdoc />
    public partial class pic2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData2",
                table: "Products",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData2",
                table: "Products");
        }
    }
}
