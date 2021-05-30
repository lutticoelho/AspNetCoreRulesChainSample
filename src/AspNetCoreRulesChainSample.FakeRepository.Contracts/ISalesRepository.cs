namespace AspNetCoreRulesChainSample.FakeRepository.Contracts
{
    public interface ISalesRepository
    {
        bool IsCouponAvailable(string couponCode);
    }
}