using System;

namespace ShoppingCart.Domain.Discount.Campaigns
{
    public class CampaignNotCreatedException : Exception
    {
        public CampaignNotCreatedException(string message) : base(message)
        {
        }
    }
}