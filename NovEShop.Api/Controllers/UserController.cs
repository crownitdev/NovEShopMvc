using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Users.Queries;
using System.Threading.Tasks;

namespace NovEShop.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IBroker _broker;

        public UserController(
            IBroker broker)
        {
            _broker = broker;
        }

        // https://localhost:5001/api/users/paging?pageNumber=1&pageSize=10&keyword=
        [HttpGet]
        public async Task<IActionResult> GetAllUsersPaging([FromQuery] GetAllUsersPagingQuery request)
        {

            var users = await _broker.Query(request);
            return Ok(users);
        }
    }
}
