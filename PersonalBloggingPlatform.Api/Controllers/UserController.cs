using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatform.Api.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
