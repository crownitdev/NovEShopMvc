using NovEShop.Data;
using NovEShop.Handler.Infrastructure;
using NovEShop.Share.Exceptions.Products;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Commands
{
    public class UpdateProductPriceCommand : ICommand<bool>
    {
        public int ProductId { get; set; }
        public decimal NewPrice { get; set; }
    }

    public class UpdateProductPriceCommandHandler : ICommandHandler<UpdateProductPriceCommand, bool>
    {
        private readonly NovEShopDbContext _dbContext;

        public UpdateProductPriceCommandHandler(NovEShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(UpdateProductPriceCommand request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                throw new ProductNotFoundException($"Không tìm thấy sản phẩm Id: {request.ProductId}");
            }

            product.Price = request.NewPrice;
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
