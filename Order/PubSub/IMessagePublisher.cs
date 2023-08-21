namespace Order.PubSub
{
public interface IMessagePublisher
    {
        Task OnReservedEventAsync(ReservedEvent reservedEvent);
    }
}