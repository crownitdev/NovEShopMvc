using System.Collections.Generic;

namespace NovEShop.Handler.Commons
{
    public class Response
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public bool IsSucceed { get; set; }
    }

    public class Response<T> : Response
    {
        public Response()
        {
        }

        public Response(T data)
            :base()
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
