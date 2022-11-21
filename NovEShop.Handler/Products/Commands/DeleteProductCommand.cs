using NovEShop.Data;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Share.Exceptions.Products;
using NovEShop.Share.Helpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Commands
{
    public class DeleteProductCommand : ICommand<DeleteProductCommandResponse>
    {
        public int ProductId { get; set; }
    }

    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, DeleteProductCommandResponse>
    {
        private readonly NovEShopDbContext _db;
        private readonly IFileStorageHelper _fileStorageHelper;

        public DeleteProductCommandHandler(NovEShopDbContext db,
            IFileStorageHelper fileStorageHelper)
        {
            _db = db;
            _fileStorageHelper = fileStorageHelper;
        }

        public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteProductCommandResponse();

            var product = await _db.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                throw new ProductNotFoundException($"Không tìm thấy sản phẩm: {request.ProductId}");
            }

            // Delete its images
            var images = _db.ProductImages.Where(i => i.ProductId == request.ProductId);

            foreach (var image in images)
            {
                await _fileStorageHelper.DeleteFileAsync(image.ImagePath);
            }

            _db.ProductImages.RemoveRange(images.ToList());
            _db.Products.Remove(product);
            var result = await _db.SaveChangesAsync();

            if (result > 0)
            {
                response.IsSucceed = true;
                response.Message = "Xoá sản phẩm thành công";
            }
            else
            {
                response.IsSucceed = false;
                response.Message = "Có lỗi xảy ra khi xoá sản phẩm";
            }

            return response;
        }
    }

    public class DeleteProductCommandResponse : Response
    {
    }
}
