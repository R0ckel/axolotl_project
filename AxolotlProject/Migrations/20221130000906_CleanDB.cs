using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxolotlProject.Migrations
{
    public partial class CleanDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Moderators");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Moderators",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moderators", x => new { x.UserId, x.Category });
                });
        }
    }
}
