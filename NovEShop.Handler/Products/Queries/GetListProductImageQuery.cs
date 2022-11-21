using Microsoft.EntityFrameworkCore;
using NovEShop.Data;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Dtos;
using System.Collections.Generic;
using System.Linq;
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
        private readonly NovEShopDbContext _db;

        public GetListProductImageQueryHandler(NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<GetListProductImageQueryResponse> Handle(GetListProductImageQuery request, CancellationToken cancellationToken)
        {
            var data =  await _db.ProductImages.Where(x => x.ProductId == request.ProductId)
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    CreatedAt = i.CreatedAt,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder
                }).ToListAsync();

            var response = new GetListProductImageQueryResponse()
            {
                Data = data,
                Message = $"Lấy hình ảnh sản phẩm {request.ProductId} thành công",
                IsSucceed = true
            };

            return response;
        }
    }

    public class GetListProductImageQueryResponse : Response<ICollection<ProductImageViewModel>>
    {
        public GetListProductImageQueryResponse()
        {
        }

        public GetListProductImageQueryResponse(ICollection<ProductImageViewModel> data)
            : base(data)
        {
        }
    }
}
