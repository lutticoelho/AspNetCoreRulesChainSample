using AspNetCoreRulesChainSample.FakeRepository.Contracts;
using AspNetCoreRulesChainSample.Model.RulesContext;
using RulesChain;
using System.Linq;

namespace AspNetCoreRulesChainSample.Rules.ShoppingCartRules
{
    public class IsValidCupomRule : Rule<ApplyDiscountContext>
    {
        private ISalesRepository _salesRepository;

        public IsValidCupomRule(Rule<ApplyDiscountContext> next, ISalesRepository salesRepository) : base(next)
        {
            _salesRepository = salesRepository;
        }

        public override ApplyDiscountContext Run(ApplyDiscountContext context)
        {
            // Gets 7% of discount;
            var myDiscount = context.Context.Items.Sum(i => i.Price * 0.07M);
            context = _next.Invoke(context) ?? context;

            // Only apply first order disccount if the discount applied by the other rules are smaller than this
            if (myDiscount > context.DiscountApplied)
            {
                context.DiscountApplied = myDiscount;
                context.DiscountTypeApplied = "Cupom";
            }

            return context;
        }

        public override bool ShouldRun(ApplyDiscountContext context)
        {
            return !string.IsNullOrWhiteSpace(context.Context.CupomCode) 
                && context.Context.Items?.Count > 1 
                && _salesRepository.IsCupomAvaliable(context.Context.CupomCode);
        }
    }
}
