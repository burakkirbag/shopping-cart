using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using ShoppingCart.Domain.Catalog.Products;
using ShoppingCart.Domain.Common;
using ShoppingCart.Domain.Discount;
using ShoppingCart.Domain.Discount.Campaigns;
using ShoppingCart.Domain.Discount.Coupons;
using ShoppingCart.Domain.Shipment;

namespace ShoppingCart.Domain.Basket
{
    public class Cart : AggregateRoot
    {
        #region [props]

        public virtual bool CampaignApplied => _campaignApplied;
        public virtual bool CouponApplied => _couponApplied;

        public IReadOnlyList<CartItem> Items => _items.ToImmutableList();
        protected virtual List<CartItem> _items { get; set; }

        private readonly IDeliveryCostCalculator _deliveryCostCalculator;

        #endregion

        #region [private members]

        private bool _campaignApplied = false;
        private bool _couponApplied = false;

        #endregion

        #region [.ctor]

        public Cart(IDeliveryCostCalculator deliveryCostCalculator)
        {
            Id = Guid.NewGuid();

            _deliveryCostCalculator = deliveryCostCalculator;
            _items = new List<CartItem>();
        }

        #endregion

        #region [Item Methods]

        public virtual void AddItem(Product product, int quantity)
        {
            if (product == null)
                throw new CartItemNotAddedException("Sepete eklemek istediğiniz ürünü belirtmelisiniz.");

            if (quantity <= 0)
                throw new CartItemNotAddedException("Ürün adeti en az 1 olmalıdır.");

            var item = _items.FirstOrDefault(x => x.Product.Id == product.Id);
            if (item != null)
            {
                _items.Remove(item);
            }

            item = new CartItem(product, quantity);
            _items.Add(item);
        }

        public virtual void RemoveItem(Product product)
        {
            if (product == null)
                throw new CartItemNotRemovedException(
                    $"Sepetten silmek istediğiniz ürünü belirtmelisiniz.");

            var findItem = _items.FirstOrDefault(x => x.Product.Id == product.Id);
            if (findItem == null)
                throw new CartItemNotRemovedException(
                    $"{product.Title} başlıklı ürün, sepetinizde bulunamadı.");

            _items.Remove(findItem);
        }

        public virtual void Clear()
        {
            _items.Clear();
        }

        #endregion

        #region [Discount Methods]

        public virtual void ApplyCampaigns(params Campaign[] campaigns)
        {
            if (campaigns == null || !campaigns.Any())
                throw new CartCampaignNotAppliedException("Kampanya bilgisi belirtmelisiniz.");

            if (_campaignApplied)
                throw new CartCampaignNotAppliedException("Sepete, bir kere kampanya uygulayabilirsiniz.");

            var willBeAppliedCampaign = FindWillBeAppliedCampaign(campaigns.ToList());
            if (willBeAppliedCampaign == null)
                return;

            _campaignApplied = ApplyCampaign(willBeAppliedCampaign);
        }

        public virtual void ApplyCoupon(Coupon coupon)
        {
            if (coupon == null)
                throw new CartCouponNotAppliedException("Kupon bilgisi belirtmelisiniz.");

            if (_couponApplied)
                throw new CartCouponNotAppliedException("Sepette, bir adet kupon kullabilirsiniz.");

            if (GetTotalAmountAfterDiscounts() < coupon.MinCartAmount)
                throw new CartCouponNotAppliedException(
                    $"Kuponu kullanabilmeniz için sepet tutarınız en az {coupon.MinCartAmount} TL olmalıdır.");

            switch (coupon.DiscountType)
            {
                case DiscountType.Amount:
                {
                    var itemDiscount = coupon.DiscountAmount / _items.Count;

                    _items.ForEach(item => item.ApplyCouponDiscount(itemDiscount));
                    break;
                }
                case DiscountType.Rate:
                {
                    foreach (var item in _items)
                    {
                        var itemDiscount = (item.TotalPriceAfterDiscounts * coupon.DiscountAmount) / 100;
                        item.ApplyCouponDiscount(itemDiscount);
                    }

                    break;
                }
                default:
                    throw new CartCouponNotAppliedException("Geçersiz kupon.");
            }

            _couponApplied = true;
        }

        #endregion

        #region [Amount Calculated Methods]

        public virtual double GetTotalAmount()
        {
            return _items.Sum(x => x.TotalPrice);
        }

        public virtual double GetTotalAmountAfterDiscounts()
        {
            return GetTotalAmount() - GetTotalDiscount();
        }

        public virtual double GetCouponDiscount()
        {
            return _items.Sum(x => x.CouponDiscount);
        }

        public virtual double GetCampaignDiscount()
        {
            return _items.Sum(x => x.CampaignDiscount);
        }

        public virtual double GetTotalDiscount()
        {
            return GetCampaignDiscount() + GetCouponDiscount();
        }

        public virtual double GetDeliveryCost()
        {
            return _deliveryCostCalculator.CalculateFor(this);
        }

        public virtual double GetLastAmount()
        {
            return GetTotalAmountAfterDiscounts() + GetDeliveryCost();
        }

        #endregion

        public int GetNumberOfDeliveries()
        {
            return _items.GroupBy(item => item.Product.Category.Id).Count();
        }

        public int GetNumberOfProducts()
        {
            return _items.Count;
        }

        public string Print()
        {
            var builder = new StringBuilder();
            var groups = _items.GroupBy(item => item.Product.Category.Id).ToList();

            builder.AppendLine(
                $"{"Kategori",20}  {"Ürün Adı",20}  {"Adet",20}  {"Birim Fiyatı",20}  {"Bürüt Toplam",20}  {" Uygulanan İndirim",20}  {"Toplam Fiyat",20}");
            builder.AppendLine(
                $"{"--------",20}  {"--------",20}  {"----",20}  {"-----------",20}  {"-------------",20}  {"----------------",20}  {"------------",20}");
            foreach (var cartItem in groups.SelectMany(@group => @group))
            {
                builder.AppendLine(
                    $"{cartItem.Product.Category.Title,20} {cartItem.Product.Title,20} {cartItem.Quantity,20} {cartItem.UnitPrice + " TL",20} {cartItem.TotalPrice + " TL",20} {cartItem.TotalDiscount + " TL",20} {cartItem.TotalPriceAfterDiscounts + " TL",20}\t");
            }

            builder.AppendLine($"\t\t\t");
            builder.AppendLine($"{"Bürüt Tutar ",10} {": ",13} {GetTotalAmount() + "TL ",15}");
            builder.AppendLine($"{"Toplam İndirim ",10} {": ",10} {" - " + GetTotalDiscount() + "TL ",15}");
            builder.AppendLine($"{"Teslimat Tutarı ",10} {": ",9} {GetDeliveryCost() + "TL ",15}");
            builder.AppendLine($"{"Toplam Tutar ",10} {": ",12} {GetLastAmount() + "TL ",15}");
            return builder.ToString();
        }

        #region [Helper Methods]

        private Campaign FilterCampaign(IGrouping<Guid, Campaign> campaigns, DiscountType filterType)
        {
            return campaigns
                .Where(x => x.DiscountType == filterType &&
                            _items
                                .Where(x2 => x2.Product.Category.Id == x.Category.Id)
                                .Sum(x2 => x2.Quantity) > x.ProductCount)
                .OrderByDescending(x => x.DiscountAmount)
                .FirstOrDefault();
        }

        private double CalculateDiscountAmountForCampaign(Campaign campaign)
        {
            double discountAmount = 0;

            var cartItems = _items
                .Where(x => x.Product.Category.Id == campaign.Category.Id)
                .ToList();

            switch (campaign.DiscountType)
            {
                case DiscountType.Amount:
                    discountAmount = campaign.DiscountAmount;
                    break;
                case DiscountType.Rate:
                    double totalAmount = cartItems.Sum(x => x.TotalPrice);
                    discountAmount = (totalAmount * campaign.DiscountAmount) / 100;
                    break;
                default:
                    discountAmount = 0;
                    break;
            }

            return discountAmount;
        }

        private Campaign FindWillBeAppliedCampaign(List<Campaign> campaigns)
        {
            Dictionary<Campaign, double> validCampaigns = new Dictionary<Campaign, double>();

            var categoryCampaigns = campaigns.GroupBy(c => c.Category.Id).ToList();
            foreach (var campaignGroup in categoryCampaigns)
            {
                double discountAmountForAmountType = 0;
                double discountAmountForRateType = 0;

                Campaign amountCampaign = FilterCampaign(campaignGroup, DiscountType.Amount);
                Campaign rateCampaign = FilterCampaign(campaignGroup, DiscountType.Rate);

                if (amountCampaign != null)
                    discountAmountForAmountType = CalculateDiscountAmountForCampaign(amountCampaign);

                if (rateCampaign != null)
                    discountAmountForRateType = CalculateDiscountAmountForCampaign(rateCampaign);

                if (discountAmountForAmountType == 0 && discountAmountForRateType == 0)
                    return null;

                if (discountAmountForAmountType < discountAmountForRateType)
                {
                    validCampaigns.Add(rateCampaign, discountAmountForRateType);
                }
                else if (discountAmountForAmountType > discountAmountForRateType)
                {
                    validCampaigns.Add(amountCampaign, discountAmountForAmountType);
                }
                else if (discountAmountForAmountType == discountAmountForRateType)
                {
                    validCampaigns.Add(rateCampaign, discountAmountForRateType);
                }
                else
                {
                    throw new CartCampaignNotAppliedException("Kampanya tutarı hesaplanamadı.");
                }
            }

            if (validCampaigns.Count <= 0)
                return null;

            var maxDiscountCampaign = validCampaigns
                .OrderByDescending(x => x.Value)
                .FirstOrDefault();

            return maxDiscountCampaign.Key;
        }

        private bool ApplyCampaign(Campaign campaign)
        {
            var result = false;

            var cartItems = _items
                .Where(x => x.Product.Category.Id == campaign.Category.Id)
                .ToList();

            switch (campaign.DiscountType)
            {
                case DiscountType.Amount:
                {
                    var itemDiscountForAmount = campaign.DiscountAmount / cartItems.Count;
                    cartItems.ForEach(item => item.ApplyCampaignDiscount(itemDiscountForAmount));
                    result = true;
                    break;
                }
                case DiscountType.Rate:
                {
                    foreach (var item in cartItems)
                    {
                        var itemDiscountForRate = (item.TotalPrice * campaign.DiscountAmount) / 100;
                        item.ApplyCampaignDiscount(itemDiscountForRate);
                        result = true;
                    }

                    break;
                }
            }

            return result;
        }

        #endregion
    }
}