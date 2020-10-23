using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientFactory.Services
{
    public class TypedOrderService
    {
        private HttpClient httpClient;

        public TypedOrderService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> Get()
        {
            return await this.httpClient.GetStringAsync("/weatherforecast");
        }
    }
}
