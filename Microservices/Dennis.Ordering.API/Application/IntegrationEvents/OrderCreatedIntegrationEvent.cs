namespace Dennis.Ordering.API.Application.IntegrationEvents
{
    public class OrderCreatedIntegrationEvent
    {
        private long id;

        public OrderCreatedIntegrationEvent(long id)
        {
            this.id = id;
        }
    }

    //public class OrderCreatedIntegrationEvent
    //{
    //    private long id;

    //    public OrderCreatedIntegrationEvent(long id)
    //    {
    //        this.id = id;
    //    }
    //}
}