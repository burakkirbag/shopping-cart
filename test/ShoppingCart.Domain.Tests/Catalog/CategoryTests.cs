using System;
using ShoppingCart.Domain.Catalog.Categories;
using Shouldly;
using Xunit;

namespace ShoppingCart.Domain.Tests.Catalog
{
    public class CategoryTests
    {
        [Fact]
        public void Should_Create_New()
        {
            var food = new Category("Food");
            food.Id.ShouldNotBe(Guid.Empty);
            food.Title.ShouldBe("Food");
        }

        [Fact]
        public void Title_Is_Null_Or_Empty_Should_Throw_Exception()
        {
            Should.Throw<CategoryNotCreatedException>(() => new Category(string.Empty))
                .Message.ShouldBe("Kategori başlığı belirtmelisiniz.");
        }
    }
}