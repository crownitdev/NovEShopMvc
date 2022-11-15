using EcommerceWebApp.Core.Exceptions.Products;
using EcommerceWebApp.Data;
using EcommerceWebApp.Handler.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace EcommerceWebApp.Handler.Products.Commands
{
    public class UpdateProductStockCommand : ICommand<bool>
    {
        public int ProductId { get; set; }
        public int NewStock { get; set; }
    }

    public class UpdateProductStockCommandHandler : ICommandHandler<UpdateProductStockCommand, bool>
    {
        private readonly EcommerceAppDbContext _db;

        public UpdateProductStockCommandHandler(EcommerceAppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _db.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                throw new ProductNotFoundException($"Sản phẩm Id {request.ProductId} không tồn tại");
            }

            product.Stock = request.NewStock;
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
