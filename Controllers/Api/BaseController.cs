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
        protected readonly IDbConnection _connection;

        public BaseController(IDbConnection connection)
        {
            _connection = connection;
        }
        protected internal virtual void HandleErrorRecord(string errorMessage, Exception ex)
        {
            LoggerService.LogError(errorMessage, ex);
        }
        protected internal virtual string HandleError(string errorMessage, Exception ex)
        {
            LoggerService.LogError(errorMessage, ex);
            var errorResult = new ResultData<object> { state = false, errorMsg = errorMessage };
            return errorResult.ToJson();
        }
    }
}
