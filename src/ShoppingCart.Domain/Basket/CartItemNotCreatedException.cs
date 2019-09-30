using System;

namespace ShoppingCart.Domain.Basket
{
    public class CartItemNotCreatedException : Exception
    {
        public CartItemNotCreatedException(string message) : base(message)
        {
        }
    }
}