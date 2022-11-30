using System;

namespace NovEShop.Share.Exceptions
{
    public class NovEShopException : Exception
    {
        public NovEShopException()
        {
        }

        public NovEShopException(string message)
            :base(message)
        {
        }

        public NovEShopException(string message, Exception innerException)
            :base(message, innerException)
        {
        }
    }
}
