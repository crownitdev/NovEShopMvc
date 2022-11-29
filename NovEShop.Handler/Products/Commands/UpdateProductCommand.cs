using Microsoft.EntityFrameworkCore;
using NovEShop.Data;
using NovEShop.Data.Models;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Dtos;
using NovEShop.Share.Exceptions.Products;
using NovEShop.Share.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Commands
{
    public class UpdateProductCommand : ProductUpdateRequest, ICommand<UpdateProductCommandResponse>
    {
    }

    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductCommandResponse>
    {
        private readonly NovEShopDbContext _db;
        private readonly IFileStorageHelper _fileStorageHelper;

        public UpdateProductCommandHandler(NovEShopDbContext db,
            IFileStorageHelper fileStorageHelper)
        {
            _db = db;
            _fileStorageHelper = fileStorageHelper;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateProductCommandResponse();

            var product = _db.Products.Find(request.Id);
            var productTranslation = await _db.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);

            if (product == null || productTranslation == null)
            {
                throw new ProductNotFoundException($"Không thể tìm thấy sản phẩm có Id: {request.Id}");
            }

            productTranslation.Name = request.Name;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.SeoTitle = request.SeoTitle;
            productTranslation.SeoAlias = request.SeoAlias;
            productTranslation.Description = request.Description;
            productTranslation.Details = request.Details;

            // Save Image

            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _db.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await _fileStorageHelper.SaveFile(request.ThumbnailImage);
                }
            }

            var saveChangeState = await _db.SaveChangesAsync();
            response.AffectResult = saveChangeState;

            if (saveChangeState > 0)
            {
                response.Message = $"Cập nhật sản phẩm {request.Id} thành công";
                response.IsSucceed = true;
            }
            else
            {
                response.Message = $"Cập nhật sản phẩm {request.Id} thất bại";
                response.IsSucceed = false;
            }

            return response;
        }
    }

    public class UpdateProductCommandResponse : Response
    {
        public int AffectResult { get; set; }
        public UpdateProductCommandResponse()
        { }

        public UpdateProductCommandResponse(string message, bool isSucceed)
        {
            this.Message = message;
            this.IsSucceed = isSucceed;
        }
    }
}
