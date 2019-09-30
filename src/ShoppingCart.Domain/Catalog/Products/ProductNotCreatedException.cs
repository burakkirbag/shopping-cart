
using System;

namespace ShoppingCart.Domain.Catalog.Products
{
    public class ProductNotCreatedException : Exception
    {
        public ProductNotCreatedException(string message) : base(message)
        {
        }
    }
}