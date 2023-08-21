namespace Order
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrder(CreateOrder createOrder);
    }
}