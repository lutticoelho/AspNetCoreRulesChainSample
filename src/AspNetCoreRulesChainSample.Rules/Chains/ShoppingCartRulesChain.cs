using RulesChain;
using AspNetCoreRulesChainSample.Model;
using AspNetCoreRulesChainSample.Model.RulesContext;
using AspNetCoreRulesChainSample.Rules.ShoppingCartRules;
using System;

namespace AspNetCoreRulesChainSample.Rules.Chains
{
    public class ShoppingCartRulesChain
    {
        public ShoppingCart ApplyDiscountOnShoppingCart(ShoppingCart shoppingCart)
        {
            var shoppingCartRuleChain = new RuleChain<ApplyDiscountContext>()
                .Use<IsValidCupomRule>()
                .Use<BirthdayDiscountRule>()
                .Use<FirstOrderDiscountRule>()
                .Build();

            var shoppingCartRuleContext = new ApplyDiscountContext(shoppingCart);
            shoppingCartRuleContext.Properties["IsFirstOrder"] = true;
            shoppingCartRuleContext.ClientBirthday = DateTime.UtcNow;
            
            shoppingCartRuleContext = shoppingCartRuleChain.Invoke(shoppingCartRuleContext);

            shoppingCart.Discount = shoppingCartRuleContext.DiscountApplied;
            shoppingCart.DiscountType = shoppingCartRuleContext.DiscountTypeApplied;
            return shoppingCart;
        }
    }
}
