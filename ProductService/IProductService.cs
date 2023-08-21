namespace ProductService
{
    public interface IProductService
    {
        // Task<List<Product>> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(int id);

        Task InsertAsync(Product product);
    }
}
