using AspNetCoreRulesChainSample.Model.RulesContext;
using RulesChain;
using System.Linq;

namespace AspNetCoreRulesChainSample.Rules.ShoppingCartRules
{
    public class FirstOrderDiscountRule : Rule<ApplyDiscountContext>
    {
        public FirstOrderDiscountRule(Rule<ApplyDiscountContext> next) : base(next)
        { }

        public override ApplyDiscountContext Run(ApplyDiscountContext context)
        {
            // Gets 5% of discount;
            var myDiscount = context.Context.Items.Sum(i => i.Price * 0.05M);
            context = _next.Invoke(context) ?? context;

            // Only apply first order disccount if the discount applied by the other rules are smaller than this
            if (myDiscount > context.DiscountApplied)
            {
                context.DiscountApplied = myDiscount;
                context.DiscountTypeApplied = "First Order Discount";
            }

            return context;
        }

        public override bool ShouldRun(ApplyDiscountContext context)
        {
            return (bool)(context.Properties["IsFirstOrder"] ?? false);
        }
    }
}
