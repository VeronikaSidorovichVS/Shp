namespace Shops.Models
{
    public class modelcs
    {
        public class User
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public bool IsAdmin { get; set; }
            public string userName { get; set; }
            public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        }
        public class CartItem
        {
            public int Id { get; set; }
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Category { get; set; }
            public int Year { get; set; }
        }
        public class Order
        {
            public int Id { get; set; }
            public User User { get; set; }
            public DateTime OrderDate { get; set; }
            public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
            public int UserId { get; internal set; }
            public OrderData OrderData { get; internal set; }

            internal static void Add(OrderItem orderItem)
            {
                throw new NotImplementedException();
            }
        }

        public class OrderItem
        {
            public int Id { get; set; }
            public Product Product { get; set; }
            public int ProductId { get; internal set; }
            public int Quantity { get; internal set; }
        }

        public class OrderData
        {
            public int Id { get; set; }
            public int Items { get; set; }

            public string Name { get; set; }
            public string Address { get; set; }
        }
    }
}
