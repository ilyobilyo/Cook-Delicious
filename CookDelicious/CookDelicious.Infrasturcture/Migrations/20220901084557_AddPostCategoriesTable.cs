using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookDelicious.Infrastructure.Data.Migrations
{
    public partial class AddPostCategoriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PostCategoryId",
                table: "ForumPosts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PostCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumPosts_PostCategoryId",
                table: "ForumPosts",
                column: "PostCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPosts_PostCategories_PostCategoryId",
                table: "ForumPosts",
                column: "PostCategoryId",
                principalTable: "PostCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPosts_PostCategories_PostCategoryId",
                table: "ForumPosts");

            migrationBuilder.DropTable(
                name: "PostCategories");

            migrationBuilder.DropIndex(
                name: "IX_ForumPosts_PostCategoryId",
                table: "ForumPosts");

            migrationBuilder.DropColumn(
                name: "PostCategoryId",
                table: "ForumPosts");
        }
    }
}
