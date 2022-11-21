using System;

namespace NovEShop.Share.Exceptions.Products
{
    public class ProductImageNotFoundException : Exception
    {
        public ProductImageNotFoundException()
        { }

        public ProductImageNotFoundException(string message)
            : base(message)
        {
        }

        public ProductImageNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
