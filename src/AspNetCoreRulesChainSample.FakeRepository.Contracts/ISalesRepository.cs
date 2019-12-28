namespace AspNetCoreRulesChainSample.FakeRepository.Contracts
{
    public interface ISalesRepository
    {
        bool IsCupomAvaliable(string cupomCode);
    }
}