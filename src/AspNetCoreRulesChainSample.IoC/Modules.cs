using AspNetCoreRulesChainSample.FakeRepository;
using AspNetCoreRulesChainSample.FakeRepository.Contracts;
using AspNetCoreRulesChainSample.Rules.Chains;
using AspNetCoreRulesChainSample.Rules.ShoppingCartRules;
using Microsoft.Extensions.DependencyInjection;
using RulesChain;
using RulesChain.Contracts;

namespace AspNetCoreRulesChainSample.IoC
{
    public static class Modules
    {
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<ISalesRepository, SalesRepository>();
        }

        public static void AddRules(this IServiceCollection services)
        {
            services.AddScoped<IsValidCouponRule>();
            services.AddScoped<FirstOrderDiscountRule>();
            services.AddScoped<BirthdayDiscountRule>();
            services.AddScoped<ShoppingCartRulesChain>();
            services.AddScoped(typeof(IRuleChain<>), typeof(RuleChain<>));
        }
    }
}
