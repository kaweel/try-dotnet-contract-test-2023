using System.Collections.Concurrent;

namespace Provider.Products
{
    public class FakeProductService : IProductService
    {

        private ConcurrentDictionary<int, Product> _products = new();

        public FakeProductService()
        {

        }

        // public Task<List<Product>> GetAllProductsAsync()
        // {
        //     return Task.FromResult(_products);
        // }

        public Task<Product> GetProductByIdAsync(int id) { 
            return Task.FromResult(_products[id]);
        }

        public Task InsertAsync(Product product)
        {
            _products[product.id] = product;
            return Task.CompletedTask;
        }

        public Task removeAllAsync(){
            _products.Clear();
            return Task.CompletedTask;
        }
    }

}