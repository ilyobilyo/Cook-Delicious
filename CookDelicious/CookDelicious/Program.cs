using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Contracts.Admin.Product;
using CookDelicious.Core.Contracts.Comments;
using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Contracts.Common.DishTypes;
using CookDelicious.Core.Contracts.Forum;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Contracts.Recipes;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Services.Admin;
using CookDelicious.Core.Services.Comments;
using CookDelicious.Core.Services.Common.Categories;
using CookDelicious.Core.Services.Common.DishTypes;
using CookDelicious.Core.Services.Forum;
using CookDelicious.Core.Services.Products;
using CookDelicious.Core.Services.Recipes;
using CookDelicious.Core.Services.User;
using CookDelicious.Infrastructure.Data;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.ModelBinders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IApplicationDbRepository, ApplicationDbRepository>()
    .AddScoped<IUserServiceAdmin, UserServiceAdmin>()
    .AddScoped<IUserService, UserService>()
    .AddScoped<IDishTypeServiceAdmin, DishTypeServiceAdmin>()
    .AddScoped<IProductService, ProductService>()
    .AddScoped<IProductServiceAdmin, ProductServiceAdmin>()
    .AddScoped<ICategoryServiceAdmin, CategoryServiceAdmin>()
    .AddScoped<ICategoryService, CategoryService>()
    .AddScoped<IDishTypeService, DishTypeService>()
    .AddScoped<IRecipeService, RecipeService>()
    .AddScoped<IForumService, ForumService>()
    .AddScoped<IForumServiceAdmin, ForumServiceAdmin>()
    .AddScoped<ICommentService, CommentService>()
    .AddScoped<ICommentServiceAdmin, CommentServiceAdmin>();

builder.Services.AddAutoMapper(typeof(RecipeMapping),
    typeof(UserMapping), 
    typeof(CategoryMapping),
    typeof(DishTypeMapping),
    typeof(ProductMapping),
    typeof(RatingMapping),
    typeof(ForumMapping));

builder.Services.AddAutoMapper(typeof(ProductMapping));

builder.Services.AddControllersWithViews()
    .AddMvcOptions(o =>
    {
        o.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider(FormatingConstants.DefaultFormat));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
