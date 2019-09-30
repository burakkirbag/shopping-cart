using System;

namespace ShoppingCart.Domain.Catalog.Categories
{
    public class CategoryNotCreatedException : Exception
    {
        public CategoryNotCreatedException(string message) : base(message)
        {
        }
    }
}