using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dennis.Mobile.ApiAggregator.Services
{
    public class OrderServiceClient
    {
        IHttpClientFactory _httpClietFactory;

        public OrderServiceClient(IHttpClientFactory httpClietFactory)
        {
            this._httpClietFactory = httpClietFactory;
        }

        public async Task Get()
        {
            var client = this._httpClietFactory.CreateClient();
            await client.GetAsync("https://localhost:5001/api/orders");
        }
    }
}
