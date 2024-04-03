namespace Shops.Models
{
    public class Type
    {
        public Guid typeId { get; set; }
        public string typeName { get; set; } = null!;
        public List<Product> products { get; set; } = [];

    }
}
