using System.Collections.Concurrent;

namespace ProductService
{
    public class ProductService : IProductService
    {        
        private readonly ConcurrentDictionary<int, Product> _products = new();

        public ProductService()
        {
            _products[9] = new Product(9, "CL500", "Motorcycle", "InStock");
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            return Task.FromResult(_products[id]);
        }

        public Task InsertAsync(Product product)
        {
            _products[product.Id] = product;
            return Task.CompletedTask;
        }
    }


}
