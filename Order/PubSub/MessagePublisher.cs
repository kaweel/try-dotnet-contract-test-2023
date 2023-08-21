namespace Order.PubSub
{
    public class MessagePublisher : IMessagePublisher
    {
        public Task OnReservedEventAsync(ReservedEvent reservedEvent)
        {
            return Task.CompletedTask;
        }
    }
}