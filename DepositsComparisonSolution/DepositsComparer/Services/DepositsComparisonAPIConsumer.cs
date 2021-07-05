namespace DepositsComparer.Services
{
    using System.Threading.Tasks;
    using Configuration;
    using DepositsComparison.Data.Public;
    using DepositsComparisonDomainLogic.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using RestSharp;

    public class DepositsComparisonAPIConsumer : IDepositsComparisonAPIConsumer
    {
        private const string GetAllBankProductsPath = "BankProducts/GetAllBankProducts";
        private const string GetAllDepositsPath = "Deposits/GetAllDeposits";
        private const string GetFilteredDepositsPath = "Deposits/GetFilteredDeposits";
        
        private readonly IOptions<APIConsumerSettings> _settings;

        public DepositsComparisonAPIConsumer(IOptions<APIConsumerSettings> settings)
        {
            _settings = settings;
        }

        public async Task<GetAllBankProductsResponse> GetAllBankProductsAsync()
        {
            var client = new RestClient(_settings.Value.Url + "/" + GetAllBankProductsPath);
            
            var response = await client.ExecuteAsync<GetAllBankProductsResponse>(new RestRequest());
            return response.Data;
        }

        public async Task<GetAllDepositsResponsе> GetAllDepositsAsync()
        {
            var client = new RestClient(_settings.Value.Url + "/" + GetAllDepositsPath);
            
            var response = await client.ExecuteAsync<GetAllDepositsResponsе>(new RestRequest());
            return response.Data;
        }

        [HttpPost(nameof(GetFilteredDepositsResponse))]
        public async Task<GetFilteredDepositsResponse> GetFilteredDepositsAsync(decimal amount, Currency currency, InterestType interestType, int periodInMonths)
        {
            var client = new RestClient(_settings.Value.Url);

            var requestModel = new GetFilteredDepositsRequest
            {
                Amount = amount,
                Currency = currency,
                InterestType = interestType,
                PeriodInMonths = periodInMonths
            };

            var request = new RestRequest($"/{GetFilteredDepositsPath}", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddJsonBody(requestModel);
            
            var response = await client.ExecuteAsync<GetFilteredDepositsResponse>(request);
            return response.Data;
        }
    }
}