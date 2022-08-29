using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookDelicious.Infrastructure.Data.Migrations
{
    public partial class RemoveSubCategoryAndCookingTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_CookingTimes_CookingTimeId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_SubCategories_SubCaregoryId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "CookingTimes");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_CookingTimeId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_SubCaregoryId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "CookingTimeId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "SubCaregoryId",
                table: "Recipes");

            migrationBuilder.AddColumn<string>(
                name: "CookingTime",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CookingTime",
                table: "Recipes");

            migrationBuilder.AddColumn<Guid>(
                name: "CookingTimeId",
                table: "Recipes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SubCaregoryId",
                table: "Recipes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CookingTimes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CookingTimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CookingTimeId",
                table: "Recipes",
                column: "CookingTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_SubCaregoryId",
                table: "Recipes",
                column: "SubCaregoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_CookingTimes_CookingTimeId",
                table: "Recipes",
                column: "CookingTimeId",
                principalTable: "CookingTimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_SubCategories_SubCaregoryId",
                table: "Recipes",
                column: "SubCaregoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
