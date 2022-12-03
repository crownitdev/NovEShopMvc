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
        private readonly IValidator<UpdateUserCommand> _updateValidator;

        public UserController(IConfiguration configuration,
            IUserApiClient userApiClient,
            IValidator<CreateUserCommand> createValidator,
            IValidator<UpdateUserCommand> updateValidator)
        {
            _configuration = configuration;
            _userApiClient = userApiClient;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
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
            else
            {
                ModelState.AddModelError("", result.Message);
            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var response = await _userApiClient.GetUserById(new GetUserByIdQuery { Id = id, TokenAuth = HttpContext.Session.GetString("Token") });
            if (!response.IsSucceed)
            {
                ModelState.AddModelError("", response.Message);
            }
            else
            {
                var updateRequest = new UpdateUserCommand()
                {
                    Id = response.Data.Id,
                    Email = response.Data.Email,
                    PhoneNumber = response.Data.PhoneNumber,
                    FirstName = response.Data.FirstName,
                    LastName = response.Data.LastName,
                    Dob = response.Data.Dob
                };

                return View(updateRequest);
            }    

            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserCommand request)
        {
            var validate = _updateValidator.Validate(request);
            if (!validate.IsValid)
            {
                validate.AddToModelState(ModelState);
                return View();
            }
            request.TokenAuth = HttpContext.Session.GetString("Token");
            var result = await _userApiClient.UpdateUser(request.Id, request);
            if (result.IsSucceed)
            {
                return RedirectToAction("Index");
            }
            return View(request);
        }
    }   
}
