using System;

namespace ShoppingCart.Domain.Basket
{
    public class CartCampaignNotAppliedException : Exception
    {
        public CartCampaignNotAppliedException(string message) : base(message)
        {
        }
    }
}