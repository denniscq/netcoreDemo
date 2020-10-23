using DotNetCore.CAP;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dennis.Ordering.API.Application.IntegrationEvents
{
    public class SubScriberService : ISubscriberService, ICapSubscribe
    {
        IMediator _mediator;

        public SubScriberService(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [CapSubscribe("OrderPaymentSuccessed")]
        public void OrderPaymentSuccessed(OrderPaymentSuccessedIntegrationEvent @event)
        {
            Console.WriteLine("---- OrderPaymentSuccessed ----");

            //logic
        }

        [CapSubscribe("OrderCreated")]
        public void OrderCreated(OrderCreatedIntegrationEvent @event)
        {
            Console.WriteLine("---- OrderCreated ----");

            //logic
        }
    }
}
