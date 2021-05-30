using System.Linq;
using System.Threading.Tasks;
using RulesChain;
using RulesChain.Contracts;
using AspNetCoreRulesChainSample.FakeRepository.Contracts;
using AspNetCoreRulesChainSample.Model.RulesContext;

namespace AspNetCoreRulesChainSample.Rules.ShoppingCartRules
{
    public class IsValidCouponRule : Rule<ApplyDiscountContext>
    {
        private readonly ISalesRepository _salesRepository;

        public IsValidCouponRule(RuleHandlerDelegate<ApplyDiscountContext> next, ISalesRepository salesRepository) : base(next)
        {
            _salesRepository = salesRepository;
        }

        public override async Task Run(ApplyDiscountContext context)
        {
            // Gets 7% of discount;
            var myDiscount = context.Context.Items.Sum(i => i.Price * 0.07M);
            await Next(context);

            // Only apply first order discount if the discount applied by the other rules are smaller than this
            if (myDiscount > context.DiscountApplied)
            {
                context.DiscountApplied = myDiscount;
                context.DiscountTypeApplied = "Cupom";
            }
        }

        public override bool ShouldRun(ApplyDiscountContext context)
        {
            return !string.IsNullOrWhiteSpace(context.Context.CupomCode) 
                && context.Context.Items?.Count > 1 
                && _salesRepository.IsCouponAvailable(context.Context.CupomCode);
        }
    }
}
