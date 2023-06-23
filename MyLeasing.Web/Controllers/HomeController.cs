using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyLeasing.Web.Models;

namespace MyLeasing.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NotAuthorized()
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return View();
        }

        [Route("/Error/404")]
        public IActionResult Error404()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View();
        }
    }
}
