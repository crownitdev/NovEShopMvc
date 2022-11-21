using NovEShop.Data;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Dtos;
using NovEShop.Share.Exceptions.Products;
using NovEShop.Share.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Commands
{
    public class UpdateProductImageCommand : ProductImageUpdateRequest, ICommand<UpdateProductImagesCommandResponse>
    {
        public int ImageId { get; set; }
    }

    public class UpdateProductImagesCommandHandler : ICommandHandler<UpdateProductImageCommand, UpdateProductImagesCommandResponse>
    {
        private readonly NovEShopDbContext _db;
        private readonly IFileStorageHelper _fileStorageHelper;

        public UpdateProductImagesCommandHandler(NovEShopDbContext db,
            IFileStorageHelper fileStorageHelper)
        {
            _db = db;
            _fileStorageHelper = fileStorageHelper;
        }

        public async Task<UpdateProductImagesCommandResponse> Handle(UpdateProductImageCommand request, CancellationToken cancellationToken)
        {
            var productImage = await _db.ProductImages.FindAsync(request.ImageId);
            if (productImage == null)
            {
                throw new ProductImageNotFoundException($"Không thể tìm thấy hình ảnh {request.ImageId}");
            }

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await _fileStorageHelper.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }

            _db.ProductImages.Update(productImage);
            
            var response = new UpdateProductImagesCommandResponse()
            {
                Data = await _db.SaveChangesAsync(),
                Message = "Cập nhật ảnh sản phẩm thành công",
                IsSucceed = true,
            };

            return response;
        }
    }

    public class UpdateProductImagesCommandResponse : Response<int>
    {

    }
}
