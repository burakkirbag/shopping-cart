using System;
using ShoppingCart.Domain.Common;

namespace ShoppingCart.Domain.Catalog.Categories
{
    public class Category : Entity
    {
        #region [props]

        public string Title { get; protected set; }

        #endregion

        #region [.ctor]

        protected Category()
        {
        }

        public Category(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new CategoryNotCreatedException("Kategori başlığı belirtmelisiniz.");

            Id = Guid.NewGuid();
            Title = title;
        }

        #endregion
    }
}