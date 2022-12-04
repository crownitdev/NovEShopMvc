using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovEShop.Handler.Categories.Queries;
using NovEShop.Handler.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovEShop.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IBroker _broker;

        public CategoriesController(
            IBroker broker)
        {
            _broker = broker;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string languageId)
        {
            var response = await _broker.Query(new GetAllCategoriesQuery { LanguageId = languageId });
            
            if (!response.IsSucceed)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
