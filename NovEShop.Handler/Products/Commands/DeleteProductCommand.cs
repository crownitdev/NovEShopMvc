using EcommerceWebApp.Core.Exceptions.Products;
using EcommerceWebApp.Data;
using EcommerceWebApp.Handler.Infrastructure;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EcommerceWebApp.Handler.Products.Commands
{
    public class DeleteProductCommand : ICommand<DeleteProductCommandResponse>
    {
        public int ProductId { get; set; }
    }

    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, DeleteProductCommandResponse>
    {
        private readonly EcommerceAppDbContext _db;

        public DeleteProductCommandHandler(EcommerceAppDbContext db)
        {
            _db = db;
        }

        public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteProductCommandResponse();

            var product = await _db.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                throw new ProductNotFoundException($"Không tìm thấy sản phẩm: {request.ProductId}");
            }

            _db.Products.Remove(product);
            var result = await _db.SaveChangesAsync();

            if (result > 0)
            {
                response.IsSucceed = true;
                response.Message = "Xoá sản phẩm thành công";
            }
            else
            {
                response.IsSucceed = false;
                response.Message = "Có lỗi xảy ra khi xoá sản phẩm";
            }

            return response;
        }
    }

    public class DeleteProductCommandResponse
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public bool IsSucceed { get; set; }
    }
}
