using MusicStore.Models;
using MusicStore.Repositories;
using MusicStore.Services;
using static System.Console;

namespace MusicStore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var productRepo = new ProductRepository();
            var clientRepo = new ClientRepository();
            var saleService = new SaleService(productRepo);
            ShowMenu(productRepo, clientRepo, saleService);
        }

        static void ShowMenu(IProductRepository productRepo, IClientRepository clientRepo, SaleService saleService)
        {
            while (true)
            {
                WriteLine("\n=== МАГАЗИН МУЗЫКАЛЬНЫХ ИНСТРУМЕНТОВ ===");
                WriteLine("1. Показать товары");
                WriteLine("2. Показать клиентов");
                WriteLine("3. Добавить клиента");
                WriteLine("4. Оформить продажу");
                WriteLine("5. Показать продажи");
                WriteLine("6. Добавить товар");
                WriteLine("0. Выход");
                Write("Выберите действие: ");
                var choice = ReadLine();
                switch (choice)
                {
                    case "1": ShowProducts(productRepo); break;
                    case "2": ShowClients(clientRepo); break;
                    case "3": AddClient(clientRepo); break;
                    case "4": CreateSale(productRepo, saleService, clientRepo); break;
                    case "5": ShowSales(saleService); break;
                    case "6": AddProduct(productRepo); break;
                    case "0": return;
                    default: WriteLine("Неверный выбор!"); break;
                }
            }
        }

        static void ShowProducts(IProductRepository repo)
        {
            WriteLine("\n--- ТОВАРЫ ---");
            foreach (var product in repo.GetAll())
            {
                WriteLine($"{product.Id}. {product.Name} - {product.Price} руб. (остаток: {product.StockQuantity})");
            }
        }

        static void ShowClients(IClientRepository repo)
        {
            WriteLine("\n--- КЛИЕНТЫ ---");
            foreach (var client in repo.GetAll())
            {
                WriteLine($"{client.Id}. {client.FullName} - {client.Phone}");
            }
        }

        static void AddClient(IClientRepository repo)
        {
            var client = new Client();
            Write("ФИО: "); client.FullName = ReadLine()!;
            Write("Телефон: "); client.Phone = ReadLine()!;
            Write("Email: "); client.Email = ReadLine()!;
            repo.Add(client);
            WriteLine("Клиент добавлен!");
        }

        static void CreateSale(IProductRepository productRepo, SaleService service, IClientRepository clientRepo)
        {
            ShowClients(clientRepo);
            Write("\nID клиента: ");
            var clientId = int.Parse(ReadLine()!);
            if (clientRepo.GetById(clientId) == null)
            {
                WriteLine("Клиент не найден!");
                return;
            }

            var items = new List<(int, int)>();
            while (true)
            {
                ShowProducts(productRepo);
                Write("\nID товара (0 для завершения): ");
                var productId = int.Parse(ReadLine()!);
                if (productId == 0) break;
                var product = productRepo.GetById(productId);
                if (product == null)
                {
                    WriteLine("Товар не найден!");
                    continue;
                }
                Write($"Количество (в наличии: {product.StockQuantity}): ");
                var quantity = int.Parse(ReadLine()!);
                if (quantity > product.StockQuantity)
                {
                    WriteLine("Недостаточно товара на складе!");
                    continue;
                }
                items.Add((productId, quantity));
            }

            if (items.Count == 0)
            {
                WriteLine("Продажа не оформлена --- не выбраны товары.");
                return;
            }

            try
            {
                var sale = service.CreateSale(clientId, items);
                WriteLine($"Продажа оформлена! Сумма: {sale.TotalAmount:C}");
            }
            catch (Exception ex)
            {
                WriteLine($"Ошибка при оформлении продажи: {ex.Message}");
            }
        }

        static void ShowSales(SaleService service)
        {
            WriteLine("\n--- ПРОДАЖИ ---");
            foreach (var sale in service.GetAllSales())
            {
                WriteLine($"№{sale.Id} от {sale.SaleDate:dd.MM.yyyy}, клиент {sale.ClientId}, сумма: {sale.TotalAmount} руб.");
            }
        }

        static void AddProduct(IProductRepository repo)
        {
            var product = new Product();
            Write("Название: "); product.Name = ReadLine()!;
            Write("Цена: "); product.Price = decimal.Parse(ReadLine()!);
            Write("Остаток: "); product.StockQuantity = int.Parse(ReadLine()!);
            Write("Категория: "); product.Category = ReadLine()!;
            repo.Add(product);
            WriteLine("Товар добавлен!");
        }
    }
}