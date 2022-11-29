using NovEShop.Handler.Commons;
using System;

namespace NovEShop.Handler.Paginations.Dtos
{
    public class PaginationResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }


        public PaginationResponse(
            T data,
            int pageNumber = 1,
            int pageSize = 10)
            : base(data)
        {
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
        }

        public PaginationResponse(
            T data,
            PaginationFilter paginationFilter)
            : base(data)
        {
            PageNumber = paginationFilter.PageNumber;
            PageSize = paginationFilter.PageSize;
        }

        public PaginationResponse(
            T data,
            int totalRecords,
            int totalPages,
            int pageNumber = 1,
            int pageSize = 10)
            :this(data, pageNumber, pageSize)
        {
            this.TotalPages = totalPages;
            this.TotalRecords = totalRecords;
        }

        public PaginationResponse(T data,
            PaginationFilter paginationFilter,
            int totalRecords,
            int totalPages)
            :this(data, paginationFilter.PageNumber, paginationFilter.PageSize, totalPages, totalRecords)
        {
        }
    }
}
