
using System.Net.Http.Json;

namespace TestFromGitToMongo.Services.ChainService
{
    public class ChainServiceClient : IChainServiceClient
    {
        private readonly BikeAPIClient _bikesClient;

        public ChainServiceClient(BikeAPIClient bikesClient)
        {
            _bikesClient = bikesClient;
        }

        public List<ChainSummaryDTO    > Chains { get; set; }  = new List<ChainSummaryDTO>();
        public ChainSummaryDTO Chain { get; set; } = new ChainSummaryDTO();

        public async Task<ServiceResponse<int>> AddChain(Chain request)
        {
            return await _bikesClient.AddChain(request);

            //var result = await _http.PostAsJsonAsync("api/Chain/addChain", request);

            //return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }

        public async Task GetChains(int bikeId)
        {

            Chains = await _bikesClient.GetChains(bikeId);
            //var result = await _http.GetFromJsonAsync<ServiceResponse<List<ChainSummaryDTO>>>($"api/Chain/bike/{bikeId}");

            //if (result != null && result.Data != null)
            //    Chains = result.Data;

        }

        public async Task GetChain(int chainId)
        {
            Chain = await _bikesClient.GetChain(chainId);
            //var result = await _http.GetFromJsonAsync<ServiceResponse<ChainSummaryDTO>>($"api/Chain/{chainId}");

            //if (result != null && result.Data != null)
            //    Chain = result.Data;

        }

    }
}
