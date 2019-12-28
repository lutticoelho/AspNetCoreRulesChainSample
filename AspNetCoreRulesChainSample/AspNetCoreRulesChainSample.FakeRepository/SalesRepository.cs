using AspNetCoreRulesChainSample.FakeRepository.Contracts;
using System;
using System.Linq;

namespace AspNetCoreRulesChainSample.FakeRepository
{
    public class SalesRepository : ISalesRepository
    {
        private static readonly string[] avaliableCupom = new[] { "cupom-001", "cupom-002", "cupom-003", "cupom-005" };

        public bool IsCupomAvaliable(string cupomCode)
        {
            return avaliableCupom.Contains(cupomCode.ToLower());
        }
    }
}
