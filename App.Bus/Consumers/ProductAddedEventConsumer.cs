using App.Domain.Event;
using MassTransit;

namespace App.Bus.Consumers
{
    public class ProductAddedEventConsumer() : IConsumer<ProductAddedEvent>
    {
        public Task Consume(ConsumeContext<ProductAddedEvent> context)
        {
            Console.WriteLine($"Product added event consumed: {context.Message.Id} - {context.Message.Name} - {context.Message.Price}");
            return Task.CompletedTask;
        }
    }
}
