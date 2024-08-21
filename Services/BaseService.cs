using DemoVenueRental.Global;
using DemoVenueRental.Models;

namespace DemoVenueRental.Services
{
    public class BaseService
    {
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
