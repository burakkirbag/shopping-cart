using System;
using ShoppingCart.Domain.Catalog.Categories;
using ShoppingCart.Domain.Common;

namespace ShoppingCart.Domain.Discount.Campaigns
{
    public class Campaign : Entity
    {
        #region [props]        

        public Category Category { get; protected set; }
        public int ProductCount { get; protected set; }
        public double DiscountAmount { get; protected set; }
        public DiscountType DiscountType { get; protected set; }

        #endregion

        #region [.ctor]

        public Campaign(Category category, double discountAmount, int productCount, DiscountType discountType)
        {
            if (category == null)
                throw new CampaignNotCreatedException("Kampanyanın uygulanacağı kategoriyi belirtmelisiniz.");

            if (discountAmount <= 0)
                throw new CampaignNotCreatedException("İndirim miktarı 0'dan büyük olmalıdır.");

            if (productCount <= 0)
                throw new CampaignNotCreatedException("Ürün sayısı en az 1 olmalıdır.");

            Id = Guid.NewGuid();
            Category = category;
            ProductCount = productCount;
            DiscountAmount = discountAmount;
            DiscountType = discountType;
        }

        #endregion
    }
}