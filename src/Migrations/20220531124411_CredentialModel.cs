using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyRepoWebApp.Migrations
{
    public partial class CredentialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CredentialModel",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    ActivationCode = table.Column<Guid>(nullable: true),
                    Verified = table.Column<bool>(nullable: false),
                    RememberMe = table.Column<bool>(nullable: false),
                    Admin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialModel", x => x.UserId);
                });

        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CredentialModel");

            migrationBuilder.DropTable(
                name: "FolderModel");

            migrationBuilder.DropTable(
                name: "Upload");
        }
    }
}
