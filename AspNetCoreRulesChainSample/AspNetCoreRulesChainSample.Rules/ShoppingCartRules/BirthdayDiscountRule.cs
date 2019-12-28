using AspNetCoreRulesChainSample.Model.RulesContext;
using RulesChain;
using System;
using System.Linq;

namespace AspNetCoreRulesChainSample.Rules.ShoppingCartRules
{
    public class BirthdayDiscountRule : Rule<ApplyDiscountContext>
    {
        public BirthdayDiscountRule(Rule<ApplyDiscountContext> next) : base(next)
        { }

        public override ApplyDiscountContext Run(ApplyDiscountContext context)
        {
            // Gets 10% of discount;
            var birthDayDiscount = context.Context.Items.Sum(i => i.Price * 0.1M);
            context = _next.Invoke(context);

            // Only apply birthday disccount if the discount applied by the other rules are smaller than this
            if (birthDayDiscount > context.DiscountApplied)
            {
                context.DiscountApplied = birthDayDiscount;
                context.DiscountTypeApplied = "Birthday Discount";
            }

            return context;
        }

        public override bool ShouldRun(ApplyDiscountContext context)
        {
            var dayAndMonth = context.ClientBirthday.ToString("ddMM");
            var todayDayAndMonth = DateTime.Now.ToString("ddMM");
            return dayAndMonth == todayDayAndMonth;
        }
    }
}
