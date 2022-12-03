using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NovEShop.Data.Models.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Roles.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovEShop.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IBroker _broker;

        public RolesController(
            IBroker broker)
        {
            _broker = broker;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _broker.Query(new GetAllRolesQuery());

            return Ok(response);
        }
    }
}
