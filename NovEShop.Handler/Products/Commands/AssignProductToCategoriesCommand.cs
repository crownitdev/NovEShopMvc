using NovEShop.Data;
using NovEShop.Data.Models;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Commands
{
    public class AssignProductToCategoriesCommand : CategoryAssignRequestDto, ICommand<AssignProductToCategoriesCommandResponse>
    {
    }

    public class AssignProductToCategoriesCommandHandler : ICommandHandler<AssignProductToCategoriesCommand, AssignProductToCategoriesCommandResponse>
    {
        private readonly NovEShopDbContext _db;

        public AssignProductToCategoriesCommandHandler(
            NovEShopDbContext db
            )
        {
            _db = db;
        }

        public async Task<AssignProductToCategoriesCommandResponse> Handle(AssignProductToCategoriesCommand request, CancellationToken cancellationToken)
        {
            var response = new AssignProductToCategoriesCommandResponse();

            var product = _db.Products.Where(x => x.Id == request.Id).FirstOrDefault();
            if (product == null)
            {
                response.Message = $"Sản phẩm {request.Id} không tồn tại";
                response.IsSucceed = false;

                return response;
            }

            var query = from c in _db.Categories
                        join pc in _db.ProductCategories on c.Id equals pc.CategoryId
                        select new { c, pc };

            var removedCategories = request.Categories.Where(x => x.Selected == false)
                .Select(x => x.CategoryId)
                .ToList();

            var removeCollection = new List<ProductCategories>();

            foreach (int removeCategoryId in removedCategories)
            {
                var productInCategory = query.Where(x => x.pc.ProductId == request.Id &&
                                                    x.pc.CategoryId == removeCategoryId)
                    .Select(x => new ProductCategories
                    {
                        CategoryId = x.pc.CategoryId,
                        ProductId = x.pc.ProductId
                    })
                    .FirstOrDefault();
                if (productInCategory == null)
                    continue;

                removeCollection.Add(productInCategory);
            }

            _db.ProductCategories.RemoveRange(removeCollection);
            await _db.SaveChangesAsync();


            // add to categories
            var addedCategories = request.Categories.Where(x => x.Selected == true)
                .Select(x => x.CategoryId)
                .ToList();


            var addCollection = new List<ProductCategories>();

            foreach (int addCategoryId in addedCategories)
            {
                var productInCategory = query.Where(x => x.pc.ProductId == request.Id &&
                                                    x.pc.CategoryId == addCategoryId)
                    .Select(x => new ProductCategories
                    {
                        CategoryId = x.pc.CategoryId,
                        ProductId = x.pc.ProductId
                    })
                    .FirstOrDefault();
                if (productInCategory != null)
                    continue;
                addCollection.Add(new ProductCategories { 
                    CategoryId = addCategoryId, 
                    ProductId = request.Id
                });
            }
            await _db.ProductCategories.AddRangeAsync(addCollection);
            await _db.SaveChangesAsync();

            response.Message = $"Chỉnh sửa danh mục cho sản phẩm {request.Id} thành công";
            response.IsSucceed = true;
            return response;
        }
    }

    public class AssignProductToCategoriesCommandResponse : Response
    {

    }
}
