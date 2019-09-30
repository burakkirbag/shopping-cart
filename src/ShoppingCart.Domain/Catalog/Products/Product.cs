using System;
using ShoppingCart.Domain.Catalog.Categories;
using ShoppingCart.Domain.Common;

namespace ShoppingCart.Domain.Catalog.Products
{
    public class Product : Entity
    {
        #region [props]

        public string Title { get; protected set; }
        public double Price { get; protected set; }
        public Category Category { get; protected set; }

        #endregion

        #region [.ctor]

        protected Product()
        {
        }

        public Product(string title, double price, Category category)
        {
            if (string.IsNullOrEmpty(title))
                throw new ProductNotCreatedException("Ürün başlığı belirtmelisiniz.");

            if (price <= 0)
                throw new ProductNotCreatedException("Ürün fiyatı en az 1 TL olmalıdır.");

            if (category == null)
                throw new ProductNotCreatedException("Ürün kategorisi belirtmelisiniz.");

            Id = Guid.NewGuid();
            Title = title;
            Price = price;
            Category = category;
        }

        #endregion
    }
}