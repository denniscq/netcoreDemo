using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientFactory.Services
{
    public class NamedOrderService
    {
        private IHttpClientFactory httpClientFactory;

        const string clientName = "NamedOrderService";

        public NamedOrderService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<string> Get()
        {
            var httpClient = this.httpClientFactory.CreateClient(clientName);
            return await httpClient.GetStringAsync("/weatherforecast");
        }
    }
}
