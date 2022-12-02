namespace NovEShop.Handler.Commons
{
    public class PagingRequestBase : RequestBase
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
