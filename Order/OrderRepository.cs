namespace Order
{
    public class OrderRepository : IOrderRepository
    {
        public Task<Order> CreateOrder(CreateOrder createOrder)
        {
            return Task.FromResult(new Order(
                1, createOrder.Price, 10, createOrder.ProductId, createOrder.ProductName, "Reserved"));
        }
    }
}