using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyRepoWebApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Upload");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Upload");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Upload");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Upload");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Upload",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Upload",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Upload",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Upload");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Upload");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Upload");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Upload",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Upload",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Upload",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Upload",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
