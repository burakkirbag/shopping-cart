using System.Linq;
using ShoppingCart.Domain.Basket;

namespace ShoppingCart.Domain.Shipment
{
    public class DeliveryCostCalculator : IDeliveryCostCalculator
    {
        private readonly double _costPerDelivery = 0;
        private readonly double _costPerProduct = 0;
        private const double _fixedCost = 2.99;

        public DeliveryCostCalculator(double costPerDelivery, double costPerProduct, double fixedCost = _fixedCost)
        {
            _costPerDelivery = costPerDelivery;
            _costPerProduct = costPerProduct;
        }

        public double CalculateFor(Cart cart)
        {
            double numberOfDeliveries = cart.GetNumberOfDeliveries();
            double numberOfProducts = cart.GetNumberOfProducts();

            return ((_costPerDelivery * numberOfDeliveries) +
                    (_costPerProduct * numberOfProducts) +
                    _fixedCost);
        }
    }
}