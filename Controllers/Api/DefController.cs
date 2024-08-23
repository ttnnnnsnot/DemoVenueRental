using DemoVenueRental.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoVenueRental.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefController : BaseController
    {
        private readonly IDefService _defService;
        public DefController(IDefService defService)
        {
            _defService = defService;
        }

        // GET: api/<DefController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Value1", "Value2", "Value3" };
        }

        // GET api/<DefController>/typeName
        [HttpGet("{typeName}")]
        public async Task<string> Get(string typeName)
        {
            try
            {
                return await _defService.GetDef(typeName);
            }
            catch(Exception ex)
            {
                return HandleError("取得資訊錯誤!", ex);
            }
        }

        // POST api/<DefController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DefController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DefController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
