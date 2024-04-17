using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Shops.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }
        public string email { get; set; } = null!;
        
        public string password{ get; set; }
     
        public string avatar { get; set; } = null!;
        public string userName { get; set; } = null!;
        public string roleName { get; set; } = null!;

        public Role? role { get; set; }
    }
    public class UserImg
    {
        public static void Main()
        {
            string json = @"
        [ 
            { 
                ""userId"": 1, 
                ""email"": ""qwerty12@gmail.com"", 
                ""password"": ""123445TfgdjF"", 
                ""avatar"": ""./img/pc.png"", 
                ""userName"": ""qwerty"", 
                ""roleName"": ""admin"",  
                ]
            }
        ]";

            // Десериализация JSON в массив объектов Product
            User[] users = JsonConvert.DeserializeObject<User[]>(json);

            // Вывод информации о каждом продукте
            foreach (User user in users)
            {
                Console.WriteLine($"User ID: {user.userId}");
                Console.WriteLine($"Email: {user.email}");
                Console.WriteLine($"Password: {user.password}");
                Console.WriteLine($"Avatar: {user.avatar}");
                Console.WriteLine($"User name: {user.userName}");
                Console.WriteLine($"Role name: {user.roleName}");
                Console.WriteLine();
            }
        }
    }
}

