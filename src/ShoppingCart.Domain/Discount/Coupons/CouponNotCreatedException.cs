using System;
using System.Runtime.Serialization;

namespace ShoppingCart.Domain.Discount.Coupons
{
    public class CouponNotCreatedException : Exception
    {
        public CouponNotCreatedException(string message) : base(message)
        {
        }
    }
}