namespace Order
{
    public interface IProductClient
    {
        // Task<List<Product>> GetAllProductsAsync();

        Task<Product> GetProductsByIdAsync(int id);
    }
}
