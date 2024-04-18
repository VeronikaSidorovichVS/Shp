using System.ComponentModel.DataAnnotations;

namespace Shops.Models
{
    public class Role {
        //public Role(string v)
        //{
        //    V = v;
        //}

        [Key]
        public string roleName { get; set; } = null!;

        public List<User> Users { get; set; } 
        //public string V { get; }
    }
}
