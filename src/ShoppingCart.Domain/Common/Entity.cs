using System;

namespace ShoppingCart.Domain.Common
{
    public abstract class Entity : IEntity
    {
        public virtual Guid Id { get; set; }
    }
}