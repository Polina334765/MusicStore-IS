using MusicStore.Models;
using MusicStore.Repositories;
using System.Collections.Generic;

namespace MusicStore.Services
{
    public class SaleService
    {
        private readonly IProductRepository _productRepository;
        private List<Sale> _sales = new();

        public SaleService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Sale CreateSale(int clientId, List<(int productId, int quantity)> items)
        {
            var sale = new Sale { ClientId = clientId, Items = new List<SaleItem>() };
            foreach (var (productId, quantity) in items)
            {
                var product = _productRepository.GetById(productId);
                if (product != null && product.StockQuantity >= quantity)
                {
                    sale.Items.Add(new SaleItem { Product = product, Quantity = quantity });
                    product.StockQuantity -= quantity;
                }
            }
            if (sale.Items.Count == 0)
                throw new InvalidOperationException("Не удалось добавить ни одного товара.");
            sale.Id = _sales.Count + 1;
            _sales.Add(sale);
            return sale;
        }

        public List<Sale> GetAllSales() => _sales;
    }
}