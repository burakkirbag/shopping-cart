using ShoppingCart.Domain.Catalog.Products;
using ShoppingCart.Domain.Common;

namespace ShoppingCart.Domain.Basket
{
    public class CartItem : Entity
    {
        #region [props]

        public Product Product { get; protected set; }
        public int Quantity { get; protected set; }
        public double CouponDiscount { get; protected set; }
        public double CampaignDiscount { get; protected set; }
        public double UnitPrice => Product.Price;
        public double TotalPrice => Product.Price * Quantity;
        public double TotalDiscount => CampaignDiscount + CouponDiscount;
        public double TotalPriceAfterDiscounts => TotalPrice - TotalDiscount;

        #endregion

        #region [.ctor]

        public CartItem(Product product, int quantity)
        {
            if (product == null)
                throw new CartItemNotCreatedException("Ürün belirtmelisiniz.");

            if (quantity <= 0)
                throw new CartItemNotCreatedException("Ürün adeti en az 1 olmalıdır.");

            Product = product;
            Quantity = quantity;
        }

        #endregion

        #region [methods]

        internal void ApplyCampaignDiscount(double amount)
        {
            CampaignDiscount = amount;
        }

        internal void ApplyCouponDiscount(double amount)
        {
            CouponDiscount = amount;
        }

        #endregion
    }
}