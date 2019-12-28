using AspNetCoreRulesChainSample.FakeRepository;
using AspNetCoreRulesChainSample.FakeRepository.Contracts;
using AspNetCoreRulesChainSample.Model.RulesContext;
using AspNetCoreRulesChainSample.Rules.Chains;
using AspNetCoreRulesChainSample.Rules.ShoppingCartRules;
using Microsoft.Extensions.DependencyInjection;
using RulesChain;

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
            services.AddScoped<IsValidCupomRule>();
            services.AddScoped<FirstOrderDiscountRule>();
            services.AddScoped<BirthdayDiscountRule>();
            services.AddScoped<ShoppingCartRulesChain>();
        }
    }
}
