using ShoppingCart.Domain.Discount;
using ShoppingCart.Domain.Discount.Coupons;
using Shouldly;
using Xunit;

namespace ShoppingCart.Domain.Tests.Discount
{
    public class CouponTests
    {
        public CouponTests()
        {
        }

        [Fact]
        public void Should_Discount_Rate_Create_New()
        {
            var coupon = new Coupon(100, 10, DiscountType.Rate);
            coupon.MinCartAmount.ShouldBe(100);
            coupon.DiscountAmount.ShouldBe(10);
            coupon.DiscountType.ShouldBe(DiscountType.Rate);
        }

        [Fact]
        public void Should_Discount_Amount_Create_New()
        {
            var coupon = new Coupon(150, 25, DiscountType.Amount);
            coupon.MinCartAmount.ShouldBe(150);
            coupon.DiscountAmount.ShouldBe(25);
            coupon.DiscountType.ShouldBe(DiscountType.Amount);
        }

        [Fact]
        public void MinCartAmount_Is_Equals_Zero_Should_Throw_Exception()
        {
            Should.Throw<CouponNotCreatedException>(() => new Coupon(250, 0, DiscountType.Amount))
                .Message.ShouldBe("İndirim miktarı 0'dan büyük olmalıdır.");
        }

        [Fact]
        public void MinCartAmount_Is_LessThan_Zero_Should_Throw_Exception()
        {
            Should.Throw<CouponNotCreatedException>(() => new Coupon(250, -10, DiscountType.Amount))
                .Message.ShouldBe("İndirim miktarı 0'dan büyük olmalıdır.");
        }
    }
}