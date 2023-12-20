

namespace TestFromGitToMongo.Services.ChainService
{
    public interface IChainServiceClient
    {
        List<ChainSummaryDTO> Chains { get; set; }
        ChainSummaryDTO Chain { get; set; }
        Task GetChains(int bikeId);
        Task GetChain(int chainId);
        Task<ServiceResponse<int>> AddChain(Chain chain);


    }
}
