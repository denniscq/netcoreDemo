using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientFactory.Services
{
    public class OrderServiceClient
    {
        private IHttpClientFactory httpClientFactory;

        public OrderServiceClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<string> Get()
        {
            var httpClient = this.httpClientFactory.CreateClient();
            return await httpClient.GetStringAsync(new Uri("https://localhost:5003/weatherforecast"));
        }
    }
}
