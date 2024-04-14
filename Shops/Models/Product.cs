using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace Shops.Models { 
public class Characteristic
    {
        [Key]
        
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Product
    {
        public Product(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public decimal Cost { get; set; }
        public bool Existing { get; set; }
        public int Discount { get; set; }
        public string Img { get; set; }
        public string Brand { get; set; }
        public Characteristic[] Characteristics { get; set; }
        public List<User> ProductsInBasket { get; set; }
    }
    public class Program
    {
        public static void Main()
        {
            string json = @"
        [
            { 
                ""productId"": 1, 
                ""quantity"": 1,
                ""type"": ""Смартфон"", 
                ""title"": ""Apple iPhone 12 64GB"", 
                ""cost"": 699, 
                ""existing"": true, 
                ""discount"": 4, 
                ""img"": ""./img/smartphone.png"", 
                ""brand"": ""Apple"", 
                ""characteristics"": [
                    { ""name"": ""Количество ядер"", ""value"": ""1080px"" }, 
                    { ""name"": ""Мощность блока"", ""value"": ""20вт"" }, 
                    { ""name"": ""экран"", ""value"": ""6.1\""/2532x1170"" }
                ]
            }, 
            { 
                ""productId"": 1, 
                ""quantity"": 1,
                ""type"": ""Смартфон"", 
                ""title"": ""Apple iPhone 12 64GB"", 
                ""cost"": 699, 
                ""existing"": true, 
                ""discount"": 4, 
                ""img"": ""./img/smartphone.png"", 
                ""brand"": ""Samsung"", 
                ""characteristics"": [
                    { ""name"": ""Количество ядер"", ""value"": ""6"" }, 
                    { ""name"": ""Мощность блока"", ""value"": ""20вт"" }, 
                    { ""name"": ""экран"", ""value"": ""6.1\""/2532x1170"" }
                ]
            }, 
            { 
                ""productId"": 3, 
                ""quantity"": 1,
                ""type"": ""Смартфон"", 
                ""title"": ""Apple iPhone 12 64GB"", 
                ""cost"": 699, 
                ""existing"": false, 
                ""discount"": 4, 
                ""img"": ""./img/pc.png"", 
                ""brand"": ""apple"", 
                ""characteristics"": [
                    { ""name"": ""Количество ядер"", ""value"": ""1080px"" }, 
                    { ""name"": ""Мощность блока"", ""value"": ""20вт"" }, 
                    { ""name"": ""экран"", ""value"": ""6.1\""/2532x1170"" }
                ]
            }
        ]";

            // Десериализация JSON в массив объектов Product
            Product[] products = JsonConvert.DeserializeObject<Product[]>(json);

            // Вывод информации о каждом продукте
            foreach (Product product in products)
            {
                Console.WriteLine($"Product ID: {product.ProductId}");
                Console.WriteLine($"Quantity: {product.Quantity}");
                Console.WriteLine($"Type: {product.Type}");
                Console.WriteLine($"Title: {product.Title}");
                Console.WriteLine($"Cost: {product.Cost}");
                Console.WriteLine($"Existing: {product.Existing}");
                Console.WriteLine($"Discount: {product.Discount}");
                Console.WriteLine($"Img: {product.Img}");
                Console.WriteLine($"Brand: {product.Brand}");
                Console.WriteLine("Characteristics:");
                foreach (Characteristic characteristic in product.Characteristics)
                {
                    Console.WriteLine($"- Name: {characteristic.Name}, Value: {characteristic.Value}");
                }
                Console.WriteLine();
            }
        }
    }
}