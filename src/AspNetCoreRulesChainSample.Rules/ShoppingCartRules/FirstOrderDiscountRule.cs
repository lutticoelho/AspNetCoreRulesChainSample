using System.Linq;
using System.Threading.Tasks;
using AspNetCoreRulesChainSample.Model.RulesContext;
using RulesChain;
using RulesChain.Contracts;

namespace AspNetCoreRulesChainSample.Rules.ShoppingCartRules
{
    public class FirstOrderDiscountRule : Rule<ApplyDiscountContext>
    {
        public FirstOrderDiscountRule(RuleHandlerDelegate<ApplyDiscountContext> next) : base(next)
        { }

        public override async Task Run(ApplyDiscountContext context)
        {
            // Gets 5% of discount;
            var myDiscount = context.Context.Items.Sum(i => i.Price * 0.05M);
            await Next(context);

            // Only apply first order disccount if the discount applied by the other rules are smaller than this
            if (myDiscount > context.DiscountApplied)
            {
                context.DiscountApplied = myDiscount;
                context.DiscountTypeApplied = "First Order Discount";
            }
        }

        public override bool ShouldRun(ApplyDiscountContext context)
        {
            return (bool)(context.Properties["IsFirstOrder"] ?? false);
        }
    }
}
