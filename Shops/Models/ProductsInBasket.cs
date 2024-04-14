using System.ComponentModel.DataAnnotations;

namespace Shops.Models
{
    public class ProductsInBasket
    {
        [Key]
        public int basketProductId { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public Basket? basket { get; set; }
        public Product? product { get; set; }
    }
}
