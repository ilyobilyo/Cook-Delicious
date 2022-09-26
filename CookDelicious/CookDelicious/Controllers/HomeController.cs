using CookDelicious.Core.Contracts.Pageing;
using CookDelicious.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPageingService pageingService;


        public HomeController(ILogger<HomeController> logger,
            IPageingService pageingService)
        {
            _logger = logger;
            this.pageingService = pageingService;
        }

        public async Task<IActionResult> Index()
        {
            var homeModel = await pageingService.GetRecipesForHomePage();

            return View(homeModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error(IList<ErrorViewModel> errors)
        {
            return View(errors);
        }
    }
}