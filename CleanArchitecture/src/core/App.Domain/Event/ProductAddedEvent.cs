namespace App.Domain.Event
{
    public record ProductAddedEvent(int Id,string Name,decimal Price):IEventOrMessage;
}
