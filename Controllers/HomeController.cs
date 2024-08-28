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

        [Authorize(Roles="Admin")]
        public IActionResult PlaceManage()
        {
            return View();
        }

        public IActionResult NoLogined(string ReturnUrl, string RefererUrl)
        {
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                ReturnUrl = "/";
            }

            TempData["NoLogin"] = (new ResultData() { message = RefererUrl }).ToSerialize();
            TempData["LoginedUrl"] = (new ResultData() { message = ReturnUrl }).ToSerialize();
            return LocalRedirect(RefererUrl);
        }

        public IActionResult AccessDenied(string ReturnUrl, string RefererUrl)
        {
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                ReturnUrl = "/";
            }

            TempData["AccessDenied"] = (new ResultData() { message = "�z�S���v��" }).ToSerialize();
            return LocalRedirect(RefererUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
