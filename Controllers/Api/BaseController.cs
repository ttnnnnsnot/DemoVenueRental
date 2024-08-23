using DemoVenueRental.Extensions;
using DemoVenueRental.Global;
using DemoVenueRental.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DemoVenueRental.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected internal virtual void HandleErrorRecord(string errorMessage, Exception ex)
        {
            LoggerService.LogError(errorMessage, ex);
        }
        protected internal virtual string HandleError(string errorMessage, Exception ex)
        {
            LoggerService.LogError(errorMessage, ex);
            var errorResult = new ResultData { state = false, errorMsg = errorMessage };
            return errorResult.ToSerialize();
        }
    }
}
