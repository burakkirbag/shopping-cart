using System.Linq;
using Moq;
using ShoppingCart.Domain.Catalog.Categories;
using ShoppingCart.Domain.Basket;
using ShoppingCart.Domain.Catalog.Products;
using ShoppingCart.Domain.Discount;
using ShoppingCart.Domain.Discount.Campaigns;
using ShoppingCart.Domain.Discount.Coupons;
using ShoppingCart.Domain.Shipment;
using Shouldly;
using Xunit;

namespace ShoppingCart.Domain.Tests.Basket
{
    public class CartTests
    {
        private Mock<IDeliveryCostCalculator> DeliveryCostCalculator;

        private Mock<Product> Product1;
        private Mock<Product> Product2;

        private Cart Cart;

        public CartTests()
        {
            DeliveryCostCalculator = new Mock<IDeliveryCostCalculator>();

            Product1 = new Mock<Product>();
            Product2 = new Mock<Product>();

            Cart = new Cart(DeliveryCostCalculator.Object);
        }

        #region [Create]

        [Fact]
        public void Should_Create_New()
        {
            var cart = new Cart(DeliveryCostCalculator.Object);
            cart.ShouldNotBeNull();
        }

        #endregion

        #region [AddItem]

        [Fact]
        public void Should_Add_Item()
        {
            Cart.AddItem(Product1.Object, 1);
            Cart.Items.Count.ShouldBe(1);
            Cart.Items.Any(x => x.Product == Product1.Object).ShouldBe(true);
        }

        [Fact]
        public void Add_Item_Product_Is_Null_Should_Throw_Exception()
        {
            Should.Throw<CartItemNotAddedException>(() => Cart.AddItem(null, 5))
                .Message.ShouldBe("Sepete eklemek istediğiniz ürünü belirtmelisiniz.");
        }

        [Fact]
        public void Add_Item_Quantity_Is_Equals_Zero_Should_Throw_Exception()
        {
            Should.Throw<CartItemNotAddedException>(() => Cart.AddItem(Product1.Object, 0))
                .Message.ShouldBe("Ürün adeti en az 1 olmalıdır.");
        }

        [Fact]
        public void Add_Item_Quantity_Is_LessThan_Zero_Should_Throw_Exception()
        {
            Should.Throw<CartItemNotAddedException>(() => Cart.AddItem(Product1.Object, -3))
                .Message.ShouldBe("Ürün adeti en az 1 olmalıdır.");
        }

        [Fact]
        public void Add_Exist_Item_Should_Remove_After_Add()
        {
            Cart.AddItem(Product1.Object, 5);
            Cart.AddItem(Product1.Object, 15);

            var findItem = Cart.Items.First(x => x.Product == Product1.Object);
            findItem.Quantity.ShouldBe(15);
        }

        #endregion

        #region [RemoveItem]

        [Fact]
        public void Should_Remove_Item()
        {
            Cart.AddItem(Product1.Object, 5);
            Cart.RemoveItem(Product1.Object);

            var findItem = Cart.Items.FirstOrDefault(x => x.Product == Product1.Object);
            findItem.ShouldBeNull();
        }

        [Fact]
        public void Remove_Item_Product_Is_Null_Should_Throw_Exception()
        {
            Should.Throw<CartItemNotRemovedException>(() => Cart.RemoveItem(null))
                .Message.ShouldBe("Sepetten silmek istediğiniz ürünü belirtmelisiniz.");
        }

        [Fact]
        public void Remove_Item_Product_Is_Not_Found_Should_Throw_Exception()
        {
            Should.Throw<CartItemNotRemovedException>(() => Cart.RemoveItem(Product2.Object))
                .Message.ShouldBe($"{Product2.Object.Title} başlıklı ürün, sepetinizde bulunamadı.");
        }

        #endregion

        #region [Clear]

        [Fact]
        public void Should_Clear()
        {
            Cart.Clear();
            Cart.Items.Count.ShouldBe(0);
        }

        #endregion

        #region [Campaign]

        [Fact]
        public virtual void Should_Apply_Campaigns()
        {
            var c1 = new Category("Food");
            var p1 = new Product("Apple", 25.0, c1);

            Cart.AddItem(p1, 5);

            var campaign1 = new Campaign(c1, 10, 6, DiscountType.Rate);
            var campaign2 = new Campaign(c1, 75, 3, DiscountType.Amount);

            Cart.ApplyCampaigns(campaign1, campaign2);

            Cart.CampaignApplied.ShouldBe(true);
            Cart.GetCampaignDiscount().ShouldBe(75);
            Cart.GetTotalDiscount().ShouldBe(75);
            Cart.GetTotalAmount().ShouldBe(125);
            Cart.GetLastAmount().ShouldBe(50);
        }

        [Fact]
        public void Apply_Campaigns_Parameter_Is_Null_Should_Throw_Exception()
        {
            Should.Throw<CartCampaignNotAppliedException>(() => Cart.ApplyCampaigns(null))
                .Message.ShouldBe("Kampanya bilgisi belirtmelisiniz.");
        }

        [Fact]
        public void Apply_Campaigns_Before_Applied_Should_Throw_Exception()
        {
            var c1 = new Category("Food");
            var p1 = new Product("Apple", 25.0, c1);

            Cart.AddItem(p1, 5);

            var campaign1 = new Campaign(c1, 10, 6, DiscountType.Rate);
            var campaign2 = new Campaign(c1, 75, 3, DiscountType.Amount);

            Cart.ApplyCampaigns(campaign1, campaign2);

            Cart.CampaignApplied.ShouldBe(true);
            Cart.GetCampaignDiscount().ShouldBe(75);
            Cart.GetTotalAmount().ShouldBe(125);
            Cart.GetTotalDiscount().ShouldBe(75);

            Should.Throw<CartCampaignNotAppliedException>(() => Cart.ApplyCampaigns(campaign1, campaign2))
                .Message.ShouldBe("Sepete, bir kere kampanya uygulayabilirsiniz.");
        }

        #endregion

        #region [Coupon]

        [Fact]
        public virtual void Should_Apply_Coupon()
        {
            var c1 = new Category("Food");
            var p1 = new Product("Apple", 25.0, c1);

            Cart.AddItem(p1, 5);

            var cpn1 = new Coupon(50, 5, DiscountType.Rate);

            Cart.ApplyCoupon(cpn1);

            Cart.CouponApplied.ShouldBe(true);
            Cart.GetCouponDiscount().ShouldBe(6.25);
            Cart.GetTotalAmount().ShouldBe(125);
            Cart.GetTotalDiscount().ShouldBe(6.25);
            Cart.GetTotalAmountAfterDiscounts().ShouldBe(118.75);
        }

        [Fact]
        public void Apply_Coupon_Parameter_Is_Null_Should_Throw_Exception()
        {
            Should.Throw<CartCouponNotAppliedException>(() => Cart.ApplyCoupon(null))
                .Message.ShouldBe("Kupon bilgisi belirtmelisiniz.");
        }

        [Fact]
        public void Apply_Coupon_Before_Applied_Should_Throw_Exception()
        {
            var c1 = new Category("Food");
            var p1 = new Product("Apple", 25.0, c1);

            Cart.AddItem(p1, 5);

            var cpn1 = new Coupon(50, 5, DiscountType.Rate);

            Cart.ApplyCoupon(cpn1);

            Cart.CouponApplied.ShouldBe(true);
            Cart.GetCouponDiscount().ShouldBe(6.25);
            Cart.GetTotalAmount().ShouldBe(125);
            Cart.GetTotalDiscount().ShouldBe(6.25);
            Cart.GetTotalAmountAfterDiscounts().ShouldBe(118.75);

            Should.Throw<CartCouponNotAppliedException>(() => Cart.ApplyCoupon(cpn1))
                .Message.ShouldBe("Sepette, bir adet kupon kullabilirsiniz.");
        }

        [Fact]
        public void Apply_Coupon_Minimum_Cart_Amount_Is_Invalid_Should_Throw_Exception()
        {
            var c1 = new Category("Food");
            var p1 = new Product("Apple", 25.0, c1);

            Cart.AddItem(p1, 5);

            var cpn1 = new Coupon(2500, 65, DiscountType.Rate);

            Should.Throw<CartCouponNotAppliedException>(() => Cart.ApplyCoupon(cpn1))
                .Message.ShouldBe(
                    $"Kuponu kullanabilmeniz için sepet tutarınız en az {cpn1.MinCartAmount} TL olmalıdır.");
        }

        #endregion

        #region [DeliveryCost]

        [Fact]
        public void Should_Get_Delivery_Cost()
        {
            Cart.AddItem(Product1.Object, 1);
            DeliveryCostCalculator.Setup(m => m.CalculateFor(Cart)).Returns(5);
            Cart.GetDeliveryCost().ShouldBe(5);
        }

        #endregion

        #region [Print]

        [Fact]
        public void Should_Print()
        {
            var c1 = new Category("Food");
            var p1 = new Product("Apple", 25.0, c1);

            Cart.AddItem(p1, 1);
            Cart.Print().ShouldNotBeNull();
            Cart.Print().ShouldNotBeEmpty();
            Cart.Print().ShouldNotBeNullOrWhiteSpace();
        }

        #endregion
    }
}