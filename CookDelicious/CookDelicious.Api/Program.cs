using CookDelicious.Core.Contracts.Admin.Product;
using CookDelicious.Core.Contracts.Blog;
using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Contracts.Common.DishTypes;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Contracts.Recipes;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Services.BlogService;
using CookDelicious.Core.Services.Common.Categories;
using CookDelicious.Core.Services.Common.DishTypes;
using CookDelicious.Core.Services.Products;
using CookDelicious.Core.Services.Recipes;
using CookDelicious.Infrastructure.Data;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddScoped<IApplicationDbRepository, ApplicationDbRepository>()
    .AddScoped<IProductService, ProductService>()
    .AddScoped<IProductServiceAdmin, ProductServiceAdmin>()
    .AddScoped<IBlogService, BlogService>()
    .AddScoped<IRecipeService, RecipeService>()
    .AddScoped<ICategoryService, CategoryService>()
    .AddScoped<IDishTypeService, DishTypeService>();

builder.Services.AddAutoMapper(typeof(ProductMapping),
    typeof(BlogMapping));
    

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Cook Delicious best Recipe web site",
        Description = "This site is the best for chefs"
    });

    o.IncludeXmlComments("CookDeliciousApiDocumentation.xml");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
