using NovEShop.Data;
using NovEShop.Data.Models;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Dtos;
using NovEShop.Share.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Commands
{
    public class AddProductImageCommand : ProductImageCreateRequest, ICommand<AddProductImageCommandResponse>
    {
        public int ProductId { get; set; }
    }

    public class AddProductImageCommandHandler : ICommandHandler<AddProductImageCommand, AddProductImageCommandResponse>
    {
        private readonly NovEShopDbContext _db;
        private readonly IFileStorageHelper _fileStorageHelper;

        public AddProductImageCommandHandler(NovEShopDbContext db,
            IFileStorageHelper fileStorageHelper)
        {
            _db = db;
            _fileStorageHelper = fileStorageHelper;
        }

        public async Task<AddProductImageCommandResponse> Handle(AddProductImageCommand request, CancellationToken cancellationToken)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                CreatedAt = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = request.ProductId,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await _fileStorageHelper.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }

            _db.ProductImages.Add(productImage);
            await _db.SaveChangesAsync();

            var response = new AddProductImageCommandResponse();
            response.Data = productImage.Id;
            response.Message = "Thêm ảnh thành công";
            response.IsSucceed = true;

            return response;
        }
    }

    public class AddProductImageCommandResponse : Response<int>
    {
    }
}
