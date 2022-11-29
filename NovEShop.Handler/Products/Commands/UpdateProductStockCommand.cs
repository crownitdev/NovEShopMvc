using NovEShop.Data;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Share.Exceptions.Products;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Commands
{
    public class UpdateProductStockCommand : ICommand<UpdateProductStockCommandResponse>
    {
        public int ProductId { get; set; }
        public int NewStock { get; set; }
    }

    public class UpdateProductStockCommandHandler : ICommandHandler<UpdateProductStockCommand, UpdateProductStockCommandResponse>
    {
        private readonly NovEShopDbContext _db;

        public UpdateProductStockCommandHandler(NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<UpdateProductStockCommandResponse> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _db.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                throw new ProductNotFoundException($"Sản phẩm Id {request.ProductId} không tồn tại");
            }

            product.Stock = request.NewStock;

            var response = new UpdateProductStockCommandResponse();
            response.AffectedRows = await _db.SaveChangesAsync();

            if (response.AffectedRows <= 0)
            {
                response.IsSucceed = false;
                response.Message = $"Cập nhật lượt xem sản phẩm {request.ProductId} thất bại";
            }
            else
            {
                response.IsSucceed = true;
                response.Message = $"Cập nhật lượt xem sản phẩm {request.ProductId} thành công";
            }

            return response;
        }
    }

    public class UpdateProductStockCommandResponse : Response
    {
        public int AffectedRows { get; set; }
    }
}
