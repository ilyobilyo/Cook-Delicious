using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookDelicious.Infrastructure.Data.Migrations
{
    public partial class AddModelsToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

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
                name: "DishTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForumPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumPosts_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "ForumComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ForumPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumComments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumComments_ForumPosts_ForumPostId",
                        column: x => x.ForumPostId,
                        principalTable: "ForumPosts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    CookingTimeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubCaregoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DishTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recipes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recipes_CookingTimes_CookingTimeId",
                        column: x => x.CookingTimeId,
                        principalTable: "CookingTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recipes_DishTypes_DishTypeId",
                        column: x => x.DishTypeId,
                        principalTable: "DishTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recipes_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeComments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeComments_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipeProducts",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeProducts", x => new { x.RecipeId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_RecipeProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeProducts_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumComments_AuthorId",
                table: "ForumComments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumComments_ForumPostId",
                table: "ForumComments",
                column: "ForumPostId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumPosts_AuthorId",
                table: "ForumPosts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeComments_AuthorId",
                table: "RecipeComments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeComments_RecipeId",
                table: "RecipeComments",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeProducts_ProductId",
                table: "RecipeProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_AuthorId",
                table: "Recipes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CategoryId",
                table: "Recipes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CookingTimeId",
                table: "Recipes",
                column: "CookingTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_DishTypeId",
                table: "Recipes",
                column: "DishTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_SubCategoryId",
                table: "Recipes",
                column: "SubCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumComments");

            migrationBuilder.DropTable(
                name: "RecipeComments");

            migrationBuilder.DropTable(
                name: "RecipeProducts");

            migrationBuilder.DropTable(
                name: "ForumPosts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "CookingTimes");

            migrationBuilder.DropTable(
                name: "DishTypes");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AspNetUsers");
        }
    }
}
