using System.ComponentModel.DataAnnotations;

namespace Shops.Models
{
    public class Type
    {
        [Key]
        public int typeId { get; set; }
        public string typeName { get; set; } = null!;
        public List<Product> products { get; set; } 
        
    }
}
