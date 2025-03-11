using App.Application.Contracts.BusServices;
using App.Domain.Event;
using MassTransit;

namespace App.Bus
{
    public class BusService(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider) : IBusService
    {
        public async Task PublishAsync<T>(T @event, CancellationToken cancellation = default) where T : IEventOrMessage
        {
            await publishEndpoint.Publish(@event, cancellation);
        }

        public async Task SendAsync<T>(T message, string queueName, CancellationToken cancellation = default) where T : IEventOrMessage
        {
            var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));
            await endpoint.Send(message, cancellation);
        }
    }
}
