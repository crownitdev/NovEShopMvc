using Microsoft.AspNetCore.Mvc;
using NovEShop.Handler.Paginations.Dtos;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PaginationBaseResponse result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
