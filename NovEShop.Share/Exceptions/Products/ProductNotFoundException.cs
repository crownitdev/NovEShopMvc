using System;

namespace NovEShop.Share.Exceptions.Products
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException()
        { }

        public ProductNotFoundException(string message)
            : base(message)
        {
        }

        public ProductNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

