using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Queries
{
    public class GetListProductImageQuery : IQuery<GetListProductImageQueryResponse>
    {
        public int ProductId { get; set; }
    }

    public class GetListProductImageQueryHandler : IQueryHandler<GetListProductImageQuery, GetListProductImageQueryResponse>
    {
        public Task<GetListProductImageQueryResponse> Handle(GetListProductImageQuery request, CancellationToken cancellationToken)
        {

        }
    }

    public class GetListProductImageQueryResponse : Response<List<ProductImageViewModel>>
    {
        public GetListProductImageQueryResponse()
        {
        }

        public GetListProductImageQueryResponse(List<ProductImageViewModel> data)
            : base(data)
        {
        }
    }
}
