using Microsoft.AspNetCore.Http;
using NovEShop.Data;
using NovEShop.Data.Models;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Dtos;
using NovEShop.Share.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Commands
{
    public class CreateProductCommand : ProductCreateRequest, ICommand<CreateProductCommandResult>
    {
        // Add user id who created the product here
    }

    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductCommandResult>
    {
        private readonly IFileStorageHelper _fileStorageHelper;
        private readonly NovEShopDbContext _dbContext;

        public CreateProductCommandHandler(NovEShopDbContext dbContext,
            IFileStorageHelper fileStorageHelper)
        {
            _fileStorageHelper = fileStorageHelper;
            _dbContext = dbContext;
        }

        public async Task<CreateProductCommandResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateProductCommandResult();
            response.Errors = new List<string>();

            // validating request data
            if (string.IsNullOrEmpty(request.Name))
            {
                response.Errors.Add("Tên sản phẩm không được bỏ trống");
                response.IsSucceed = false;
                return response;
            }

            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                CreatedAt = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        LanguageId = request.LanguageId,
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoTitle = request.SeoTitle,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                    }
                }
            };

            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = $"{request.Name}.product_thumb.{DateTime.Now}",
                        CreatedAt = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            response.Message = "Tạo sản phẩm thành công";
            response.IsSucceed = true;

            return response;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _fileStorageHelper.SaveFileAsync(file.OpenReadStream(), fileName);

            return fileName;
        }
    }

    public class CreateProductCommandResult
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public bool IsSucceed { get; set; }
    }
}
