using EcommerceWebApp.Data;
using EcommerceWebApp.Handler.Commons.Dtos;
using EcommerceWebApp.Handler.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EcommerceWebApp.Handler.Categories.Commands
{
    public class DisableCategoryCommand : ICommand<DisableCategoryCommandResponse>
    {
        public int CategoryId { get; set; }
    }

    public class DisableCategoryCommandHandler : ICommandHandler<DisableCategoryCommand, DisableCategoryCommandResponse>
    {
        private readonly EcommerceAppDbContext _db;

        public DisableCategoryCommandHandler(
            EcommerceAppDbContext db)
        {
            _db = db;
        }

        public async Task<DisableCategoryCommandResponse> Handle(DisableCategoryCommand request, CancellationToken cancellationToken)
        {
            DisableCategoryCommandResponse response = new DisableCategoryCommandResponse();

            var category = await _db.Categories.FindAsync(request.CategoryId);
            if (category == null)
            {
                response.Errors = new List<string>();
                response.Errors.Add("Danh mục không tồn tại");
                response.IsSucceed = false;
                response.Message = "Thay đổi trạng thái danh mục thất bại";

                return response;
            }

            category.IsActive = false;
            await _db.SaveChangesAsync();

            response.Message = "Thay đổi trạng thái danh mục thành công";
            response.IsSucceed = true;

            return response;
        }
    }

    public class DisableCategoryCommandResponse : Response
    {
    }
}
