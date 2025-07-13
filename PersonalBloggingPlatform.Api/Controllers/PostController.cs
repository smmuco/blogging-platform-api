using BloggingPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private readonly IPostService postService;









        public IActionResult Index()
        {
            return View();
        }
    }
}
