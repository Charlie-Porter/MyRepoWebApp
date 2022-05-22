using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyRepoWebApp.Migrations
{
    public partial class CredentaiModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "CredentialModel");

            migrationBuilder.AlterColumn<byte[]>(
                name: "contents",
                table: "Upload",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "CredentialModel",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "CredentialModel");

            migrationBuilder.AlterColumn<byte[]>(
                name: "contents",
                table: "Upload",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]));

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "CredentialModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
