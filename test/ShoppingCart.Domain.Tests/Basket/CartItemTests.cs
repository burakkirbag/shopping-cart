using Moq;
using ShoppingCart.Domain.Basket;
using ShoppingCart.Domain.Catalog.Products;
using Shouldly;
using Xunit;

namespace ShoppingCart.Domain.Tests.Basket
{
    public class CartItemTests
    {
        private Mock<Product> Product1;
        private Mock<Product> Product2;
        private Mock<Product> Product3;

        public CartItemTests()
        {
            Product1 = new Mock<Product>();
            Product2 = new Mock<Product>();
            Product3 = new Mock<Product>();
        }

        [Fact]
        public void Should_Create_New()
        {
            var cartItem = new CartItem(Product1.Object, 5);
            cartItem.Product.ShouldBe(Product1.Object);
            cartItem.Quantity.ShouldBe(5);
            cartItem.CouponDiscount.ShouldBe(0);
            cartItem.CampaignDiscount.ShouldBe(0);
            cartItem.UnitPrice.ShouldBe(Product1.Object.Price);
            cartItem.TotalPrice.ShouldBe(Product1.Object.Price * 5);
            cartItem.TotalDiscount.ShouldBe(0);
            cartItem.TotalPriceAfterDiscounts.ShouldBe(Product1.Object.Price * 5);
        }

        [Fact]
        public void Product_Is_Null_Should_Throw_Exception()
        {
            Should.Throw<CartItemNotCreatedException>(() => new CartItem(null, 5))
                .Message.ShouldBe("Ürün belirtmelisiniz.");
        }

        [Fact]
        public void ProductCount_Is_Equals_Zero_Should_Throw_Exception()
        {
            Should.Throw<CartItemNotCreatedException>(() => new CartItem(Product2.Object, 0))
                .Message.ShouldBe("Ürün adeti en az 1 olmalıdır.");
        }

        [Fact]
        public void ProductCount_Is_LessThan_Zero_Should_Throw_Exception()
        {
            Should.Throw<CartItemNotCreatedException>(() => new CartItem(Product3.Object, -25))
                .Message.ShouldBe("Ürün adeti en az 1 olmalıdır.");
        }
    }
}