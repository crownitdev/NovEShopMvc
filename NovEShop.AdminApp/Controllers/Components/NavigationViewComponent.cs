using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovEShop.AdminApp.Models;
using NovEShop.AdminApp.Services.Languages;
using NovEShop.Share.Constants;
using System.Linq;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Controllers.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly ILanguageApiClient _languageApiClient;

        public NavigationViewComponent(ILanguageApiClient languageApiClient)
        {
            _languageApiClient = languageApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await _languageApiClient.GetAll();

            var navigationViewModel = new NavigationViewModel()
            {
                CurrentLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId),
                Languages = response.Data.ToList()
            };

            return View("Default", navigationViewModel);
        }
    }
}
