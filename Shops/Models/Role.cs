using System.ComponentModel.DataAnnotations;

namespace Shops.Models
{
    public class Role {
        public string roleName { get; set; } = null!;

        public List<User> Users { get; set; } = [];

    }
}
