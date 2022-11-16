using NovEShop.Data;
using NovEShop.Data.Models;
using NovEShop.Handler.Categories.Dtos;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Categories.Commands
{
    public class CreateCategoryCommand : CreateCategoryRequest, ICommand<CreateCategoryCommandResponse>
    {
    }

    public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CreateCategoryCommandResponse>
    {
        private readonly NovEShopDbContext _db;

        public CreateCategoryCommandHandler(
            NovEShopDbContext db)
        {
            _db = db;
        }
        public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            CreateCategoryCommandResponse response = new CreateCategoryCommandResponse();

            if (string.IsNullOrEmpty(request.Name))
            {
                response.Errors = new List<string>();
                response.Errors.Add("Tên danh mục không được bỏ trống");
                response.Message = "Tạo danh mục thất bại";

                return response;
            }

            var category = new Category()
            {
                IsShowOnHome = request.IsShowOnHome,
                ParentId = request.ParentId,
                SortOrder = 1,
                CategoryTranslations = new List<CategoryTranslation>()
                {
                    new CategoryTranslation()
                    {
                        Name = request.Name,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId,
                    }
                },
                IsActive = true
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            response.Message = "Tạo danh mục thành công";
            response.IsSucceed = true;

            return response;
        }
    }

    public class CreateCategoryCommandResponse : Response
    {

    }
}
