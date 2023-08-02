using System.Collections.Generic;
using System.Threading.Tasks;

namespace Provider.Products
{
    public interface IProductService
    {
        // Task<List<Product>> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(int id);

        Task InsertAsync(Product product);
    }
}
