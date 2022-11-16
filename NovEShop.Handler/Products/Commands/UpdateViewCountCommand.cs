using MediatR;
using NovEShop.Data;
using NovEShop.Handler.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Commands
{
    public class UpdateViewCountCommand : ICommand
    {
        public int ProductId { get; set; }
    }

    public class UpdateViewCountCommandHandler : ICommandHandler<UpdateViewCountCommand>
    {
        private readonly NovEShopDbContext _db;

        public UpdateViewCountCommandHandler(NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(UpdateViewCountCommand request, CancellationToken cancellationToken)
        {
            var product = await _db.Products.FindAsync(request.ProductId);
            product.ViewCount += 1;
            await _db.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
