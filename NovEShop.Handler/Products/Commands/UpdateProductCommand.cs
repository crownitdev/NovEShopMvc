using EcommerceWebApp.Core.Exceptions.Products;
using EcommerceWebApp.Data;
using EcommerceWebApp.Handler.Commons.Dtos;
using EcommerceWebApp.Handler.Infrastructure;
using EcommerceWebApp.Handler.Products.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EcommerceWebApp.Handler.Products.Commands
{
    public class UpdateProductCommand : ProductUpdateRequest ,ICommand<UpdateProductCommandResponse>
    {
    }

    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductCommandResponse>
    {
        private readonly EcommerceAppDbContext _db;

        public UpdateProductCommandHandler(EcommerceAppDbContext db)
        {
            _db = db;
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

            var saveChangeState = await _db.SaveChangesAsync();

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
        public UpdateProductCommandResponse()
        { }

        public UpdateProductCommandResponse(string message, bool isSucceed)
        {
            this.Message = message;
            this.IsSucceed = isSucceed;
        }
    }
}
