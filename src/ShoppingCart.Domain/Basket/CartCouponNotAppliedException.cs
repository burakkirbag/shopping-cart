using System;
namespace ShoppingCart.Domain.Basket
{
    public class CartCouponNotAppliedException : Exception
    {
        public CartCouponNotAppliedException(string message) : base(message)
        {
        }
    }
}