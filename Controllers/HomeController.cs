using DemoVenueRental.Extensions;
using DemoVenueRental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DemoVenueRental.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult PlaceManage()
        {
            return View();
        }

        [Authorize(Roles="Admin")]
        public IActionResult PlaceEdit()
        {
            return View();
        }

        public IActionResult NoLogined()
        {
            TempData["NoLogined"] = (new ResultData() { message = "�Х���n�J" }).ToSerialize();
            return LocalRedirect("/");
        }

        public IActionResult AccessDenied()
        {
            TempData["AccessDenied"] = (new ResultData() { message = "�z�S���v��" }).ToSerialize();
            return LocalRedirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
