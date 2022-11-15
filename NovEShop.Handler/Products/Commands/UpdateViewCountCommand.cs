using EcommerceWebApp.Data;
using EcommerceWebApp.Handler.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EcommerceWebApp.Handler.Products.Commands
{
    public class UpdateViewCountCommand : ICommand
    {
        public int ProductId { get; set; }
    }

    public class UpdateViewCountCommandHandler : ICommandHandler<UpdateViewCountCommand>
    {
        private readonly EcommerceAppDbContext _db;

        public UpdateViewCountCommandHandler(EcommerceAppDbContext db)
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
