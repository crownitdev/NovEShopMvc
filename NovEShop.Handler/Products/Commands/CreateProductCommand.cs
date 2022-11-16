using NovEShop.Data;
using NovEShop.Data.Models;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Dtos;
using System;
using System.Collections.Generic;
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
        private readonly NovEShopDbContext _dbContext;

        public CreateProductCommandHandler(NovEShopDbContext dbContext)
        {
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

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            response.Message = "Tạo sản phẩm thành công";
            response.IsSucceed = true;

            return response;
        }
    }

    public class CreateProductCommandResult
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public bool IsSucceed { get; set; }
    }
}
