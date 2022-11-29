using MediatR;
using NovEShop.Data;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Commands
{
    public class UpdateViewCountCommand : ICommand<UpdateViewCountCommandResponse>
    {
        public int ProductId { get; set; }
    }

    public class UpdateViewCountCommandHandler : ICommandHandler<UpdateViewCountCommand, UpdateViewCountCommandResponse>
    {
        private readonly NovEShopDbContext _db;

        public UpdateViewCountCommandHandler(NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<UpdateViewCountCommandResponse> Handle(UpdateViewCountCommand request, CancellationToken cancellationToken)
        {
            var product = await _db.Products.FindAsync(request.ProductId);
            product.ViewCount += 1;

            var response = new UpdateViewCountCommandResponse();

            response.AffectedRows = await _db.SaveChangesAsync();

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

    public class UpdateViewCountCommandResponse : Response
    {
        public int AffectedRows { get; set; }
    }
}
