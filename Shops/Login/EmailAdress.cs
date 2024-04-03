

using Shops.Login;

namespace Shops.Login
{
    public class EmailAdress : SendGrid.Helpers.Mail.EmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public EmailAdress(string address)
        {
            Address = address;
        }

        public EmailAdress(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                return $"{Name} <{Address}>";
            }
            else
            {
                return Address;
            }
        }

    }
}
public class Proverkd
{
    static void Main()
    {
        EmailAdress email1 = new EmailAdress("john@example.com");
        Console.WriteLine(email1.ToString()); // Выводит "john@example.com"

        EmailAdress email2 = new EmailAdress("Jane Doe", "jane@example.com");
        Console.WriteLine(email2.ToString()); // Выводит "Jane Doe <jane@example.com>"
    }
}