using NovEShop.Share.Exceptions.Products;
using NovEShop.Share.Helpers;
using NovEShop.Data;
using NovEShop.Data.Models;
using NovEShop.Handler.Infrastructure;
using MediatR;
//using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace NovEShop.Handler.Products.Commands
{
    public class SaveFileCommand : ICommand
    {
        public int ProductId { get; set; }
        public IFormFile File { get; set; }
    }

    public class SaveFileCommandHandler : ICommandHandler<SaveFileCommand>
    {
        private readonly NovEShopDbContext _db;
        private readonly IFileStorageHelper _fileStoreHelper;

        public SaveFileCommandHandler(
            NovEShopDbContext db,
            IFileStorageHelper fileStoreHelper)
        {
            _db = db;
            _fileStoreHelper = fileStoreHelper;
        }

        public async Task<Unit> Handle(SaveFileCommand request, CancellationToken cancellationToken)
        {
            var product = await _db.Products.FindAsync(request.ProductId);

            if (product == null)
            {
                throw new ProductNotFoundException("Sản phẩm không tồn tại, không thể thêm hình ảnh");
            }

            if (request.File != null)
            {
                // Images of the product is existed
                var thumbnailImage = await _db.ProductImages.FirstOrDefaultAsync(x => x.IsDefault == true && x.ProductId == request.ProductId);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.File.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.File);
                    _db.ProductImages.Update(thumbnailImage);
                }
                else
                {
                    product.ProductImages = new List<ProductImage>()
                    {
                        new ProductImage()
                        {
                            Caption = "Thumbnail image",
                            CreatedAt = DateTime.Now,
                            FileSize = request.File.Length,
                            ImagePath = await this.SaveFile(request.File),
                            IsDefault = true,
                            SortOrder = 1,
                        }
                    };
                }
            }

            await _db.SaveChangesAsync();

            return Unit.Value;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _fileStoreHelper.SaveFileAsync(file.OpenReadStream(), fileName);

            return fileName;
        }
    }
}
