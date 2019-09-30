using Moq;
using ShoppingCart.Domain.Catalog.Categories;
using ShoppingCart.Domain.Discount;
using ShoppingCart.Domain.Discount.Campaigns;
using Shouldly;
using Xunit;

namespace ShoppingCart.Domain.Tests.Discount
{
    public class CampaignTests
    {
        private Mock<Category> ProductCategory1;

        public CampaignTests()
        {
            ProductCategory1 = new Mock<Category>();
        }

        [Fact]
        public void Should_Discount_Rate_Create_New()
        {
            var campaign = new Campaign(ProductCategory1.Object, 25, 10, DiscountType.Rate);
            campaign.Category.ShouldBe(ProductCategory1.Object);
            campaign.DiscountAmount.ShouldBe(25);
            campaign.ProductCount.ShouldBe(10);
            campaign.DiscountType.ShouldBe(DiscountType.Rate);
        }

        [Fact]
        public void Should_Discount_Amount_Create_New()
        {
            var campaign = new Campaign(ProductCategory1.Object, 150, 10, DiscountType.Amount);
            campaign.Category.ShouldBe(ProductCategory1.Object);
            campaign.DiscountAmount.ShouldBe(150);
            campaign.ProductCount.ShouldBe(10);
            campaign.DiscountType.ShouldBe(DiscountType.Amount);
        }

        [Fact]
        public void Category_Is_Null_Should_Throw_Exception()
        {
            Should.Throw<CampaignNotCreatedException>(() => new Campaign(null, 150, 10, DiscountType.Amount))
                .Message.ShouldBe("Kampanyanın uygulanacağı kategoriyi belirtmelisiniz.");
        }

        [Fact]
        public void DiscountAmount_Is_Equals_Zero_Should_Throw_Exception()
        {
            Should.Throw<CampaignNotCreatedException>(() => new Campaign(ProductCategory1.Object, 0, 10, DiscountType.Amount))
                .Message.ShouldBe("İndirim miktarı 0'dan büyük olmalıdır.");
        }

        [Fact]
        public void DiscountAmount_Is_LessThan_Zero_Should_Throw_Exception()
        {
            Should.Throw<CampaignNotCreatedException>(() => new Campaign(ProductCategory1.Object, -10, 10, DiscountType.Amount))
                .Message.ShouldBe("İndirim miktarı 0'dan büyük olmalıdır.");
        }

        [Fact]
        public void ProductCount_Is_Equals_Zero_Should_Throw_Exception()
        {
            Should.Throw<CampaignNotCreatedException>(() => new Campaign(ProductCategory1.Object, 150, 0, DiscountType.Amount))
                .Message.ShouldBe("Ürün sayısı en az 1 olmalıdır.");
        }

        [Fact]
        public void ProductCount_Is_LessThan_Zero_Should_Throw_Exception()
        {
            Should.Throw<CampaignNotCreatedException>(() => new Campaign(ProductCategory1.Object, 150, -5, DiscountType.Amount))
                .Message.ShouldBe("Ürün sayısı en az 1 olmalıdır.");
        }
    }
}