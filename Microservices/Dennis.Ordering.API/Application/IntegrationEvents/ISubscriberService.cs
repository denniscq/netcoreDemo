
namespace Dennis.Ordering.API.Application.IntegrationEvents
{
    public interface ISubscriberService
    {
        void OrderPaymentSuccessed(OrderPaymentSuccessedIntegrationEvent @event);
    }
}
