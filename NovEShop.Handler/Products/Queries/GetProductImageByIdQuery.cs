using NovEShop.Data;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Dtos;
using NovEShop.Share.Exceptions.Products;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Queries
{
    public class GetProductImageByIdQuery : IQuery<GetProductImageByIdQueryResponse>
    {
        public int ImageId { get; set; }
    }

    public class GetProductImageByIdQueryHandler : IQueryHandler<GetProductImageByIdQuery, GetProductImageByIdQueryResponse>
    {
        private readonly NovEShopDbContext _db;

        public GetProductImageByIdQueryHandler(NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<GetProductImageByIdQueryResponse> Handle(GetProductImageByIdQuery request, CancellationToken cancellationToken)
        {
            var image = await _db.ProductImages.FindAsync(request.ImageId);
            if (image == null)
            {
                throw new ProductImageNotFoundException($"Hình ảnh {request.ImageId} không tồn tại");
            }

            var data = new ProductImageViewModel()
            {
                Id = image.Id,
                Caption = image.Caption,
                ImagePath = image.ImagePath,
                FileSize = image.FileSize,
                CreatedAt = image.CreatedAt,
                IsDefault = image.IsDefault,
                SortOrder = image.SortOrder
            };

            var response = new GetProductImageByIdQueryResponse()
            {
                Data = data,
                IsSucceed = true,
                Message = "Lấy hình ảnh thành công"
            };

            return response;
        }
    }

    public class GetProductImageByIdQueryResponse : Response<ProductImageViewModel>
    {
    }
}
