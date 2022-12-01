using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxolotlProject.Migrations
{
    public partial class BanDateM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "BanDate",
                table: "Bans",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BanDate",
                table: "Bans");

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
