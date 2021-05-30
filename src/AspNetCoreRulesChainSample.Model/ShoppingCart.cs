using System;
using System.Collections.Generic;

namespace AspNetCoreRulesChainSample.Model
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Discount { get; set; }
        public string DiscountType { get; set; }
        public string CouponCode { get; set; }
        public List<ShoppingCartItem> Items { get; set; }
    }
}
