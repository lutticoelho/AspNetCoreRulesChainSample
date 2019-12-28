using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using RulesChain.Contracts;

namespace AspNetCoreRulesChainSample.Model.RulesContext
{
    public class ApplyDiscountContext : IRuleContext<ShoppingCart>
    {
        public ApplyDiscountContext()
        {
        }

        public ApplyDiscountContext(ShoppingCart shoppingCart)
        {
            Properties = new ConcurrentDictionary<string, object>();
            Context = shoppingCart;
        }

        public IDictionary<string, object> Properties { get; }

        public ShoppingCart Context { get; set; }

        public DateTime ClientBirthday { get; set; }
        public decimal DiscountApplied { get; set; }
        public string DiscountTypeApplied { get; set; }
    }
}
