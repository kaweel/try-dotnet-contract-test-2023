namespace ProductService
{
    public interface IProductService
    {
        Task<Product> GetProductByIdAsync(int id);

        Task InsertAsync(Product product);
    }
}
