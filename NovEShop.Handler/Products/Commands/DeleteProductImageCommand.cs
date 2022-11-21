using NovEShop.Data;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Share.Exceptions.Products;
using NovEShop.Share.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Commands
{
    public class DeleteProductImageCommand : ICommand<DeleteProductImagesCommandResponse>
    {
        public int ImageId { get; set; }
    }

    public class DeleteProductImagesCommandHandler : ICommandHandler<DeleteProductImageCommand, DeleteProductImagesCommandResponse>
    {
        private readonly NovEShopDbContext _db;
        private readonly IFileStorageHelper _fileStoreHelper;

        public DeleteProductImagesCommandHandler(
            NovEShopDbContext db,
            IFileStorageHelper fileStoreHelper)
        {
            _db = db;
            _fileStoreHelper = fileStoreHelper;
        }

        public async Task<DeleteProductImagesCommandResponse> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
        {
            var productImage = await _db.ProductImages.FindAsync(request.ImageId);
            if (productImage == null)
            {
                throw new ProductImageNotFoundException($"Hình ảnh {request.ImageId} không tồn tại");
            }

            await _fileStoreHelper.DeleteFileAsync(_fileStoreHelper.GetFileName(productImage.ImagePath));
            _db.ProductImages.Remove(productImage);

            var response = new DeleteProductImagesCommandResponse();
            response.Data = request.ImageId;
            response.Message = $"Xoá hình ảnh {request.ImageId} thành công";
            response.IsSucceed = true;

            return response;
        }
    }

    public class DeleteProductImagesCommandResponse : Response<int>
    {
    }
}
