namespace ProductService.PubSub
{
    public interface IMessageConsumer
    {
        Task OnReservedEventAsync(ReservedEvent reservedEvent);
    }
}