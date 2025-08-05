using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyDataLayer.Migrations
{
    /// <inheritdoc />
    public partial class uploadImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoPath",
                table: "Companies");

            migrationBuilder.AddColumn<byte[]>(
                name: "LogoBytes",
                table: "Companies",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoBytes",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "LogoPath",
                table: "Companies",
                type: "text",
                nullable: true);
        }
    }
}
