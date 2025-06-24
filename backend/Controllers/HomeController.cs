using Microsoft.AspNetCore.Mvc;

using TaskSync.Services.Interfaces;

namespace TaskSync.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IHomeVmService _homeVmService;
        public HomeController(IHomeVmService homeVmService) => _homeVmService = homeVmService;

        [HttpGet("")]
        public IActionResult Index()
        {
            var vm = _homeVmService.Build();
            return View("~/Views/Home/Index.cshtml", vm); // Return Razor View
        }
    }
}
