using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dennis.Mobile.ApiAggregator.Services
{
    public class OrderService : IOrderService
    {
        IHttpClientFactory _httpClientFactory;

        public OrderService(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public void GetOrder()
        {
            var client = this._httpClientFactory.CreateClient();
        }
    }
}
