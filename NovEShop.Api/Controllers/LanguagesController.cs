using Microsoft.AspNetCore.Mvc;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Languages.Queries;
using System.Threading.Tasks;

namespace NovEShop.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly IBroker _broker;

        public LanguagesController(
            IBroker broker)
        {
            _broker = broker;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLanguages()
        {
            var response = await _broker.Query(new GetAllLanguagesQuery());
            
            if (response.IsSucceed == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
