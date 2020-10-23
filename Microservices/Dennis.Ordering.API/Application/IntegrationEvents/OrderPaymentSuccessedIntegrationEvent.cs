namespace Dennis.Ordering.API.Application.IntegrationEvents
{
    public class OrderPaymentSuccessedIntegrationEvent
    {
        public OrderPaymentSuccessedIntegrationEvent(long orderId)
        {
            this.OrderId = orderId;
        }

        public long OrderId { get; private set; }
    }
}