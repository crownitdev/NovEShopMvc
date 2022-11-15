using EcommerceWebApp.Core.Helpers;
using EcommerceWebApp.Data;
using EcommerceWebApp.Handler.Commons.Dtos;
using EcommerceWebApp.Handler.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EcommerceWebApp.Handler.Products.Commands
{
    public class DeleteProductImagesCommand : ICommand<DeleteProductImagesCommandResponse>
    {
        public int ProductId { get; set; }
    }

    public class DeleteProductImagesCommandHandler : ICommandHandler<DeleteProductImagesCommand, DeleteProductImagesCommandResponse>
    {
        private readonly EcommerceAppDbContext _db;
        private readonly IFileStorageHelper _fileStoreHelper;

        public DeleteProductImagesCommandHandler(
            EcommerceAppDbContext db,
            IFileStorageHelper fileStoreHelper)
        {
            _db = db;
            _fileStoreHelper = fileStoreHelper;
        }

        public Task<DeleteProductImagesCommandResponse> Handle(DeleteProductImagesCommand request, CancellationToken cancellationToken)
        {

        }
    }

    public class DeleteProductImagesCommandResponse : Response
    {
    }
}
