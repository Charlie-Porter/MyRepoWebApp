using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyRepoWebApp.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "contents",
                table: "Upload",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contents",
                table: "Upload");
        }
    }
}
