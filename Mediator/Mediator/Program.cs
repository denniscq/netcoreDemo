using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mediator
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddMediatR(typeof(Program).Assembly);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mediator = serviceProvider.GetService<IMediator>();

            //mediator.Send(new MyCommend { Name = "chenqiang" });
            mediator.Publish(new MyEvent { Name = "chenqiang" });

            Console.ReadKey();
        }
    }

    #region command

    internal class MyCommend: IRequest<long>
    {
        public string Name { get; set; }
    }

    //internal class MyCommendHandler2 : IRequestHandler<MyCommend, long>
    //{
    //    public Task<long> Handle(MyCommend request, CancellationToken cancellationToken)
    //    {
    //        Console.WriteLine($"MyCommendHandler2 execute {request.Name}");
    //        return Task.FromResult(10L);
    //    }
    //}

    internal class MyCommendHandler : IRequestHandler<MyCommend, long>
    {
        public Task<long> Handle(MyCommend request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"MyCommendHandler execute {request.Name}");
            return Task.FromResult(10L);
        }
    }

    #endregion

    #region domain event

    internal class MyEvent: INotification
    {
        public string Name { get; set; }
    }

    internal class MyEventHandler : INotificationHandler<MyEvent>
    {
        public Task Handle(MyEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"MyEventHandler execute {notification.Name}");
            return Task.CompletedTask;
        }
    }
    
    internal class MyEventHandler2 : INotificationHandler<MyEvent>
    {
        public Task Handle(MyEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"MyEventHandler2 execute {notification.Name}");
            return Task.CompletedTask;
        }
    }

    #endregion
}
