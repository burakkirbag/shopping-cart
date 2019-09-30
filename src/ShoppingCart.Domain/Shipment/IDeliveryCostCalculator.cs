using ShoppingCart.Domain.Basket;

namespace ShoppingCart.Domain.Shipment
{
    public interface IDeliveryCostCalculator
    {
        double CalculateFor(Cart cart);
    }
}