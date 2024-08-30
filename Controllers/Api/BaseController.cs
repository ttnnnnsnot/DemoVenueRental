using DemoVenueRental.Extensions;
using DemoVenueRental.Global;
using DemoVenueRental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Data;

namespace DemoVenueRental.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private string GetFirstErrorMessage(ModelStateDictionary ModelState)
        {
            return ModelState.Values.SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage)
                                    .FirstOrDefault() ?? "資料有誤";
        }

        protected internal virtual string ModelStateFirstError(ModelStateDictionary ModelState)
        {
            var firstError = GetFirstErrorMessage(ModelState);
            return (new ResultData { message = firstError }).ToSerialize();
        }

        protected internal virtual string ModelStateFirstError<T>(ModelStateDictionary ModelState) where T : new()
        {
            var firstError = GetFirstErrorMessage(ModelState);
            return (new ResultData<T> { message = firstError }).ToSerialize();
        }
        protected internal virtual void HandleErrorRecord(string errorMessage, Exception ex)
        {
            LoggerService.LogError(errorMessage, ex);
        }
        protected internal virtual string HandleError(string errorMessage, Exception ex)
        {
            LoggerService.LogError(errorMessage, ex);
            var errorResult = new ResultData { state = false, message = errorMessage };
            return errorResult.ToSerialize();
        }
    }
}
