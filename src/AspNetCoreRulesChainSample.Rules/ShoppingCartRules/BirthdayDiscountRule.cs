using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreRulesChainSample.Model.RulesContext;
using RulesChain;
using RulesChain.Contracts;

namespace AspNetCoreRulesChainSample.Rules.ShoppingCartRules
{
    public class BirthdayDiscountRule : Rule<ApplyDiscountContext>
    {
        public BirthdayDiscountRule(RuleHandlerDelegate<ApplyDiscountContext> next) : base(next)
        { }

        public override async Task Run(ApplyDiscountContext context)
        {
            // Gets 10% of discount;
            var birthDayDiscount = context.Context.Items.Sum(i => i.Price * 0.1M);
            await Next(context);

            // Only apply birthday discount if the discount applied by the other rules are smaller than this
            if (birthDayDiscount > context.DiscountApplied)
            {
                context.DiscountApplied = birthDayDiscount;
                context.DiscountTypeApplied = "Birthday Discount";
            }
        }

        public override bool ShouldRun(ApplyDiscountContext context)
        {
            var dayAndMonth = context.ClientBirthday.ToString("ddMM");
            var todayDayAndMonth = DateTime.Now.ToString("ddMM");
            return dayAndMonth == todayDayAndMonth;
        }
    }
}
