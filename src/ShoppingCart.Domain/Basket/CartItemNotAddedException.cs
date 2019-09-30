using System;

namespace ShoppingCart.Domain.Basket
{
    public class CartItemNotAddedException : Exception
    {
        public CartItemNotAddedException(string message) : base(message)
        {
        }
    }
}