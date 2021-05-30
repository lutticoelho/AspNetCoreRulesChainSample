using AspNetCoreRulesChainSample.Model;
using AspNetCoreRulesChainSample.Model.RulesContext;
using AspNetCoreRulesChainSample.Rules.ShoppingCartRules;
using System;
using RulesChain.Contracts;

namespace AspNetCoreRulesChainSample.Rules.Chains
{
    public class ShoppingCartRulesChain
    {
        private readonly IRuleChain<ApplyDiscountContext> _ruleChain;
        public ShoppingCartRulesChain(IRuleChain<ApplyDiscountContext> ruleChain)
        {
            _ruleChain = ruleChain;
        }

        public ShoppingCart ApplyDiscountOnShoppingCart(ShoppingCart shoppingCart)
        {
            var shoppingCartRuleChain = _ruleChain
                .Use<IsValidCouponRule>()
                .Use<BirthdayDiscountRule>()
                .Use<FirstOrderDiscountRule>()
                .Build();

            var shoppingCartRuleContext = new ApplyDiscountContext(shoppingCart);
            shoppingCartRuleContext.Properties["IsFirstOrder"] = true;
            shoppingCartRuleContext.ClientBirthday = DateTime.UtcNow;
            
            shoppingCartRuleChain(shoppingCartRuleContext);

            shoppingCart.Discount = shoppingCartRuleContext.DiscountApplied;
            shoppingCart.DiscountType = shoppingCartRuleContext.DiscountTypeApplied;
            return shoppingCart;
        }
    }
}
