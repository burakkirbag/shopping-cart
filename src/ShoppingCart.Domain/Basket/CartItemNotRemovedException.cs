using System;

namespace ShoppingCart.Domain.Basket
{
    public class CartItemNotRemovedException : Exception
    {
        public CartItemNotRemovedException(string message) : base(message)
        {
        }
    }
}