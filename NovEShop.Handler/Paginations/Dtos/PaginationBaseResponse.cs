using System;

namespace NovEShop.Handler.Paginations.Dtos
{
    public class PaginationBaseResponse
    {
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount
        {
            get
            {
                var pageCount = (double)TotalRecords / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
        }
    }
}
