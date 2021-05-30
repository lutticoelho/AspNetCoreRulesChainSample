using AspNetCoreRulesChainSample.FakeRepository.Contracts;
using System;
using System.Linq;

namespace AspNetCoreRulesChainSample.FakeRepository
{
    public class SalesRepository : ISalesRepository
    {
        private static readonly string[] AvailableCoupons = new[] { "cupom-001", "cupom-002", "cupom-003", "cupom-005" };

        public bool IsCouponAvailable(string couponCode)
        {
            return AvailableCoupons.Contains(couponCode.ToLower());
        }
    }
}
