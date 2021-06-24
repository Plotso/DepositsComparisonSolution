namespace DepositsComparer.Services
{
    using System.Threading.Tasks;
    using Configuration;
    using DepositsComparisonDomainLogic.Contracts;
    using RestSharp;

    public class DepositsComparisonAPIConsumer : IDepositsComparisonAPIConsumer
    {
        private const string GetAllBankProductsPath = "BankProducts/GetBankProducts";
        private const string GetAllDepositsPath = "Deposits/GetAllDeposits";
        
        private readonly APIConsumerSettings _settings;

        public DepositsComparisonAPIConsumer(APIConsumerSettings settings)
        {
            _settings = settings;
        }

        public async Task<GetAllBankProductsResponse> GetAllBankProductsAsync()
        {
            var client = new RestClient(_settings.Url + "/" + GetAllBankProductsPath);
            
            var response = await client.ExecuteAsync<GetAllBankProductsResponse>(new RestRequest());
            return response.Data;
        }

        public async Task<GetAllDepositsResponsе> GetAllDeposits()
        {
            var client = new RestClient(_settings.Url + "/" + GetAllDepositsPath);
            
            var response = await client.ExecuteAsync<GetAllDepositsResponsе>(new RestRequest());
            return response.Data;
        }
    }
}