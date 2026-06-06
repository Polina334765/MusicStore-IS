using MusicStore.Models;
using System.Collections.Generic;
using System.Linq;

namespace MusicStore.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Акустическая гитара Fender", Price = 15000, StockQuantity = 5, Category = "Гитары" },
            new Product { Id = 2, Name = "Синтезатор Yamaha", Price = 25000, StockQuantity = 3, Category = "Клавишные" },
            new Product { Id = 3, Name = "Барабанная установка Pearl", Price = 45000, StockQuantity = 2, Category = "Ударные" }
        };

        public List<Product> GetAll() => _products;
        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);
        public void Add(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
        }
        public void Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.StockQuantity = product.StockQuantity;
                existing.Category = product.Category;
            }
        }
        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null) _products.Remove(product);
        }
    }
}