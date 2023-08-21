using System.Reflection.Metadata;

namespace ProductService.PubSub
{
    public class MessageConsumer : IMessageConsumer
    {
        public readonly IProductService _productService;

        public MessageConsumer(IProductService productService)
        {
            _productService = productService;
        }

        public async Task OnReservedEventAsync(ReservedEvent reservedEvent)
        {
            await _productService.ReserveProduct(reservedEvent.Id);
        }
    }
}