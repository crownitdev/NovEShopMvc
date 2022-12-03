using System.Collections.Generic;

namespace NovEShop.Handler.Paginations.Dtos
{
    public class PaginationDataResponse<T> : PaginationBaseResponse
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public bool IsSucceed { get; set; }
        public T Data { get; set; }

        public PaginationDataResponse()
        {
        }

        public PaginationDataResponse(T data)
            : base()
        {
            Data = data;
        }

    }
}
