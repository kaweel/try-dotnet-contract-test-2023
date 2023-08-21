namespace Order
{
    public interface IOrderService
    {
        Task Create(CreateOrder createOrder);
    }
}