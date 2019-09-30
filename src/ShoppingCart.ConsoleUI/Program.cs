using System;
using ShoppingCart.Domain.Catalog.Categories;
using ShoppingCart.Domain.Catalog.Products;
using ShoppingCart.Domain.Discount;
using ShoppingCart.Domain.Discount.Campaigns;
using ShoppingCart.Domain.Discount.Coupons;
using ShoppingCart.Domain.Basket;
using ShoppingCart.Domain.Shipment;

namespace ShoppingCart.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            IDeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(10, 150);

            Cart cart = new Cart(deliveryCostCalculator);

            Category food = new Category("Food");
            Product apple = new Product("Apple", 100.0, food);
            Product almond = new Product("Almonds", 150.0, food);

            Category telephone = new Category("Telephone");
            Product note9 = new Product("Samsung Note 9", 5500.0, telephone);
            Product iphone7 = new Product("Apple iPhone 7", 3500.0, telephone);

            cart.AddItem(apple, 3);
            cart.AddItem(almond, 1);
            cart.AddItem(note9, 1);
            cart.AddItem(iphone7, 1);

            Campaign campaign1 = new Campaign(food, 10, 1, DiscountType.Rate);
            Campaign campaign2 = new Campaign(food, 20, 1, DiscountType.Rate);
            cart.ApplyCampaigns(campaign1, campaign2);

            Coupon coupon1 = new Coupon(100, 10, DiscountType.Rate);
            cart.ApplyCoupon(coupon1);

            //Coupon coupon2 = new Coupon(500, 25, DiscountType.Amount);
            //cart.ApplyCoupon(coupon2);

            Console.WriteLine(cart.Print());
        }
    }
}
