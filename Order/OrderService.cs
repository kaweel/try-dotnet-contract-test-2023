using Order.PubSub;

namespace Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ProductClient _productClient;

        public OrderService(IOrderRepository orderRepository, IMessagePublisher messagePublisher, ProductClient productClient)
        {
            _orderRepository = orderRepository;
            _messagePublisher = messagePublisher;
            _productClient = productClient;
        }

        public async Task Create(CreateOrder createOrder)
        {
            // call api product name `CL 500`
            // var product = await _productClient.GetProductsByIdAsync(9);
            // booking product
            // await _messagePublisher.OnReservedEventAsync(new ReservedEvent(product.Id));
            // create order 
            // _orderRepository.CreateOrder()
            throw new NotImplementedException();
        }
    }

}