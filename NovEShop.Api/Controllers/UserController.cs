using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Users.Commands;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand request)
        {
            var result = await _broker.Command(request);
            
            if(!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // PUT: https://localhost:5001/api/users/update/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserCommand request)
        {
            var result = await _broker.Command(request);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute]GetUserByIdQuery request)
        {
            var result = await _broker.Query(request);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}/{tokenAuth}")]
        public async Task<IActionResult> Delete(int id, string tokenAuth)
        {
            var result = await _broker.Command(new DeleteUserCommand { Id = id, TokenAuth = tokenAuth });
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
