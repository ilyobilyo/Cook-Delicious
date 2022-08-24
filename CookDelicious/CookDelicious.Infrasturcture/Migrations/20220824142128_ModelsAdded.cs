using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookDelicious.Infrastructure.Data.Migrations
{
    public partial class ModelsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_SubCategories_SubCategoryId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_SubCategoryId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "Recipes");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_SubCaregoryId",
                table: "Recipes",
                column: "SubCaregoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_SubCategories_SubCaregoryId",
                table: "Recipes",
                column: "SubCaregoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_SubCategories_SubCaregoryId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_SubCaregoryId",
                table: "Recipes");

            migrationBuilder.AddColumn<Guid>(
                name: "SubCategoryId",
                table: "Recipes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_SubCategoryId",
                table: "Recipes",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_SubCategories_SubCategoryId",
                table: "Recipes",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
