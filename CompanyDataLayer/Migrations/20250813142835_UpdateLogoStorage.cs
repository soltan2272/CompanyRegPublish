using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyDataLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLogoStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoBytes",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "LogoPath",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoPath",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<byte[]>(
                name: "LogoBytes",
                table: "AspNetUsers",
                type: "bytea",
                nullable: true);
        }
    }
}
