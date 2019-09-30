using System;
using Moq;
using ShoppingCart.Domain.Catalog.Categories;
using ShoppingCart.Domain.Catalog.Products;
using Shouldly;
using Xunit;

namespace ShoppingCart.Domain.Tests.Catalog
{
    public class ProductTests
    {
        private Mock<Category> ProductCategory1;

        public ProductTests()
        {
            ProductCategory1 = new Mock<Category>();
        }

        [Fact]
        public void Should_Create_New()
        {
            var apple = new Product("Apple", 11.5, ProductCategory1.Object);
            apple.Id.ShouldNotBe(Guid.Empty);
            apple.Title.ShouldBe("Apple");
            apple.Price.ShouldBe(11.5);
            apple.Category.ShouldBe(ProductCategory1.Object);
        }

        [Fact]
        public void Title_Is_Null_Or_Empty_Should_Throw_Exception()
        {
            Should.Throw<ProductNotCreatedException>(() => new Product(string.Empty, 10.0, ProductCategory1.Object))
                .Message.ShouldBe("Ürün başlığı belirtmelisiniz.");
        }

        [Fact]
        public void Price_Is_Equals_Zero_Should_Throw_Exception()
        {
            Should.Throw<ProductNotCreatedException>(() => new Product("Apple", 0.0, ProductCategory1.Object))
                .Message.ShouldBe("Ürün fiyatı en az 1 TL olmalıdır.");
        }

        [Fact]
        public void Price_Is_LessThan_Zero_Should_Throw_Exception()
        {
            Should.Throw<ProductNotCreatedException>(() => new Product("Apple", -10, ProductCategory1.Object))
                .Message.ShouldBe("Ürün fiyatı en az 1 TL olmalıdır.");
        }

        [Fact]
        public void Category_Is_Null_Should_Throw_Exception()
        {
            Should.Throw<ProductNotCreatedException>(() => new Product("Apple", 10.0, null))
                .Message.ShouldBe("Ürün kategorisi belirtmelisiniz.");
        }
    }
}