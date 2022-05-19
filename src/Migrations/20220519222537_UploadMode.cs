using Microsoft.EntityFrameworkCore.Migrations;

namespace MyRepoWebApp.Migrations
{
    public partial class UploadMode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "owner",
                table: "Upload",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "owner",
                table: "Upload");
        }
    }
}
