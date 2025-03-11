using App.Domain.Event;

namespace App.Application.Contracts.BusServices
{
    public interface IBusService
    {
        Task PublishAsync<T>(T @event, CancellationToken cancellation = default) where T : IEventOrMessage;
        Task SendAsync<T>(T message, string queueName, CancellationToken cancellation = default) where T : IEventOrMessage;
        //Task SubscribeAsync<T>(Func<T, Task> handler) where T : class;
    }
}
