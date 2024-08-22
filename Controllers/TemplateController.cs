using Microsoft.AspNetCore.Mvc;

namespace DemoVenueRental.Controllers
{
    public class TemplateController : Controller
    {
        [HttpGet("Template/{viewName}")]
        public IActionResult GetPartialView(string viewName)
        {
            if (string.IsNullOrEmpty(viewName) || !IsValidViewName(viewName))
            {
                return NotFound();
            }

            return PartialView(viewName);
        }
        private bool IsValidViewName(string viewName)
        {
            var allowedViews = new List<string> { "_Register", "_Login", "_Test" };
            return allowedViews.Contains(viewName);
        }
    }
}
