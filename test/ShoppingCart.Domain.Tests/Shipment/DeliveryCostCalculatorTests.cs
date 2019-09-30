using Moq;
using ShoppingCart.Domain.Basket;
using ShoppingCart.Domain.Shipment;
using Shouldly;
using Xunit;

namespace ShoppingCart.Domain.Tests.Shipment
{
    public class DeliveryCostCalculatorTests
    {
        DeliveryCostCalculator DeliveryCostCalculator;

        public DeliveryCostCalculatorTests()
        {
        }

        [Fact]
        public void Should_Calculate()
        {
            DeliveryCostCalculator = new DeliveryCostCalculator(5, 10);

            var cart = new Cart(DeliveryCostCalculator);



            DeliveryCostCalculator.CalculateFor(cart).ShouldBe(2.99);
        }
    }
}