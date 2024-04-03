namespace Shops.Models
{
    public class Basket
    {
       public Guid userId { get; set; }
        public Guid basketId { get; set; }

        public User? user { get; set; }
        public List<ProductsInBasket> products { get; set; } = [];
  }
}