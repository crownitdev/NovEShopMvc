using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NovEShop.AdminApp.Services.Users;
using NovEShop.Handler.Users.Commands;
using NovEShop.Handler.Users.Queries;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IUserApiClient _userApiClient;
        private readonly IValidator<CreateUserCommand> _createValidator;

        public UserController(IConfiguration configuration,
            IUserApiClient userApiClient,
            IValidator<CreateUserCommand> createValidator)
        {
            _configuration = configuration;
            _userApiClient = userApiClient;
            _createValidator = createValidator;
        }

        public async Task<IActionResult> Index(string keyword, int pageNumber = 1, int pageSize = 10)
        {
            var token = HttpContext.Session.GetString("Token");
            var request = new GetAllUsersPagingQuery()
            {
                BearerToken = token,
                Keyword = keyword,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var data = await _userApiClient.GetAllUsersPaging(request);
            return View(data.Data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand request)
        {
            var validate = _createValidator.Validate(request);
            if (!validate.IsValid)
            {
                validate.AddToModelState(ModelState);
                return View();
            }
            request.Token = HttpContext.Session.GetString("Token");
            var result = await _userApiClient.CreateUser(request);
            if (result.IsSucceed)
            {
                return RedirectToAction("Index");
            }
            return View(request);
        }
    }   
}
