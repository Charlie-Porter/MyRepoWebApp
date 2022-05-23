using Microsoft.EntityFrameworkCore.Migrations;

namespace MyRepoWebApp.Migrations
{
    public partial class AddCredentialModelAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "admin",
                table: "CredentialModel",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "admin",
                table: "CredentialModel");
        }
    }
}
