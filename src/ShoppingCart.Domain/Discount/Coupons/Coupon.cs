using ShoppingCart.Domain.Common;

namespace ShoppingCart.Domain.Discount.Coupons
{
    public class Coupon : Entity
    {
        #region [props]

        public double MinCartAmount { get; protected set; }
        public double DiscountAmount { get; protected set; }
        public DiscountType DiscountType { get; protected set; }

        #endregion

        #region [.ctor]

        public Coupon(double minCartAmount, double discountAmount, DiscountType discountType)
        {
            if (minCartAmount < 0)
                throw new CouponNotCreatedException("Minimum sepet tutarı en az 0 TL olmalıdır.");

            if (discountAmount <= 0)
                throw new CouponNotCreatedException("İndirim miktarı 0'dan büyük olmalıdır.");

            MinCartAmount = minCartAmount;
            DiscountAmount = discountAmount;
            DiscountType = discountType;
        }

        #endregion
    }
}