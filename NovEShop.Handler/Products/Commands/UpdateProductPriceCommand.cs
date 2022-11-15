using EcommerceWebApp.Core.Exceptions.Products;
using EcommerceWebApp.Data;
using EcommerceWebApp.Handler.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace EcommerceWebApp.Handler.Products.Commands
{
    public class UpdateProductPriceCommand : ICommand<bool>
    {
        public int ProductId { get; set; }
        public decimal NewPrice { get; set; }
    }

    public class UpdateProductPriceCommandHandler : ICommandHandler<UpdateProductPriceCommand, bool>
    {
        private readonly EcommerceAppDbContext _dbContext;

        public UpdateProductPriceCommandHandler(EcommerceAppDbContext dbContext)
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
