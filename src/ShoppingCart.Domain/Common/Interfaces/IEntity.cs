using System;

namespace ShoppingCart.Domain.Common
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}