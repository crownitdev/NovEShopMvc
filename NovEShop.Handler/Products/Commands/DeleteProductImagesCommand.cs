using NovEShop.Data;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Share.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Commands
{
    public class DeleteProductImagesCommand : ICommand<DeleteProductImagesCommandResponse>
    {
        public int ProductId { get; set; }
    }

    //public class DeleteProductImagesCommandHandler : ICommandHandler<DeleteProductImagesCommand, DeleteProductImagesCommandResponse>
    //{
    //    private readonly NovEShopDbContext _db;
    //    private readonly IFileStorageHelper _fileStoreHelper;

    //    public DeleteProductImagesCommandHandler(
    //        NovEShopDbContext db,
    //        IFileStorageHelper fileStoreHelper)
    //    {
    //        _db = db;
    //        _fileStoreHelper = fileStoreHelper;
    //    }

    //    public Task<DeleteProductImagesCommandResponse> Handle(DeleteProductImagesCommand request, CancellationToken cancellationToken)
    //    {

    //    }
    //}

    public class DeleteProductImagesCommandResponse : Response
    {
    }
}
