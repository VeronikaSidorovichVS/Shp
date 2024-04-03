namespace Shops.Models
{
    public class ProductsInBasket
    {
        public Guid basketProductId { get; set; }
        public Guid productId { get; set; }
        public int quantity { get; set; }
        public Basket? basket { get; set; }
        public Product? product { get; set; }

    }
}
