using NovEShop.Data;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Share.Exceptions.Products;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Commands
{
    public class UpdateProductPriceCommand : ICommand<UpdateProductPriceCommandResponse>
    {
        public int ProductId { get; set; }
        public decimal NewPrice { get; set; }
    }

    public class UpdateProductPriceCommandHandler : ICommandHandler<UpdateProductPriceCommand, UpdateProductPriceCommandResponse>
    {
        private readonly NovEShopDbContext _dbContext;

        public UpdateProductPriceCommandHandler(NovEShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UpdateProductPriceCommandResponse> Handle(UpdateProductPriceCommand request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                throw new ProductNotFoundException($"Không tìm thấy sản phẩm Id: {request.ProductId}");
            }

            product.Price = request.NewPrice;
            product.OriginalPrice = request.NewPrice;

            var response = new UpdateProductPriceCommandResponse();

            response.AffectedRows = await _dbContext.SaveChangesAsync();

            if (response.AffectedRows <= 0)
            {
                response.IsSucceed = false;
                response.Message = $"Cập nhật giá sản phẩm {request.ProductId} thất bại";
            }
            else
            {
                response.IsSucceed = true;
                response.Message = $"Cập nhật giá sản phẩm {request.ProductId} thành công";
            }

            return response;
        }
    }

    public class UpdateProductPriceCommandResponse : Response
    {
        public int AffectedRows { get; set; }
    }
}
