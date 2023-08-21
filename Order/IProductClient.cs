namespace Order
{
    public interface IProductClient
    {
        Task<Product> GetProductsByIdAsync(int id);
    }
}
