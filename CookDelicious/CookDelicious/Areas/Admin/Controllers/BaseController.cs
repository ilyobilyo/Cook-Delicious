using CookDelicious.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    [Authorize(Roles = UserConstraints.Roles.Administrator)]
    [Area("Admin")]
    public class BaseController : Controller
    {
       
    }
}
