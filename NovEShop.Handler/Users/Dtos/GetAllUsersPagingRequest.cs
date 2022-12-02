using NovEShop.Handler.Paginations.Dtos;

namespace NovEShop.Handler.Users.Dtos
{
    public class GetAllUsersPagingRequest : PaginationFilter
    {
        public string Keyword { get; set; }
    }
}
