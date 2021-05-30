using AspNetCoreRulesChainSample.FakeRepository.Contracts;
using System.Linq;

namespace AspNetCoreRulesChainSample.FakeRepository
{
    public class SalesRepository : ISalesRepository
    {
        private static readonly string[] AvailableCoupons = { "coupon-001", "coupon-002", "coupon-003", "coupon-005" };

        public bool IsCouponAvailable(string couponCode)
        {
            return AvailableCoupons.Contains(couponCode.ToLower());
        }
    }
}
