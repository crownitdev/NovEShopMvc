using Microsoft.AspNetCore.Mvc;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Queries;
using System.Threading.Tasks;

namespace NovEShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IBroker _broker;

        public ProductController(IBroker broker)
        {
            _broker = broker;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromBody] GetAllProductPagingQuery query)
        {
            var response = await _broker.Query(query);

            return Ok(response);
        }
    }
}
