using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Shops.Models;

public class PosterApp
{
    private const string ConnectionString = "Data Source=(local);Initial Catalog=PosterSQL;Integrated Security=True";

    public static void Main()
    {
        Console.WriteLine("Welcome to PosterApp!");

        while (true)
        {
            Console.WriteLine("1. Sign up");
            Console.WriteLine("2. Log in");
            Console.WriteLine("3. Exit");
            Console.Write("Please enter your choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    SignUp();
                    break;
                case "2":
                    Login();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void SignUp()
    {
        Console.Write("Enter your email: ");
        var email = Console.ReadLine();

        //Check if email already exists
        if (CheckEmailExists(email))
        {
            Console.WriteLine("Email already exists. Please choose a different email.");
            return;
        }

        Console.Write("Enter your password: ");
        var password = Console.ReadLine();

        //Save user to database
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var query = "INSERT INTO Users (Email, Password) VALUES (@Email, @Password)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);
                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Sign up successful!");
        //Proceed to the website or perform other actions
    }


    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    private static void Login()
    {
        Console.Write("Enter your email: ");
        var email = Console.ReadLine();

        Console.Write("Enter your password: ");
        var password = Console.ReadLine();

        //Check if email and password match
        if (!CheckCredentials(email, password))
        {
            Console.WriteLine("Invalid email or password. Login failed.");
            return;
        }

        Console.WriteLine("Login successful!");
        //Proceed to the website or perform other actions
    }

    private static bool CheckEmailExists(string email)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                var count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }

    private static bool CheckCredentials(string email, string password)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);
                var count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
    private static void AddToCart(int productId, int userId)
    {
        //Check if the user already has the product in the cart
        if (CheckProductInCart(productId, userId))
        {
            //Increase the quantity by 1
            UpdateCartItemQuantity(productId, userId, 1);
        }
        else
        {
            // Add the product to the cart
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var query = "INSERT INTO CartItems (ProductId, UserId, Quantity) VALUES (@ProductId, @UserId, @Quantity)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Quantity", 1);
                    command.ExecuteNonQuery();
                }
            }
        }

        Console.WriteLine("Product added to cart.");
    }

    private static void RemoveFromCart(int productId, int userId)
    {
        // Remove the product from the cart
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var query = "DELETE FROM CartItems WHERE ProductId = @ProductId AND UserId = @UserId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Product removed from cart.");
    }

    private static void DeleteUser(int userId)
    {
        //Remove the user from the database
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var query = "DELETE FROM Users WHERE UserId = @UserId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("User deleted.");
    }

    private static void PlaceOrder(int userId)
    {
        //Get the cart items for the user
        var cartItems = GetCartItems(userId);

        //Check if the cart is empty
        if (cartItems.Count == 0)
        {
            Console.WriteLine("Cart is empty. Cannot place order.");
            return;
        }

        //Save the order to the database
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var query = "INSERT INTO Orders (UserId, OrderDate) VALUES (@UserId, @OrderDate); SELECT SCOPE_IDENTITY();";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                var orderId = (int)(decimal)command.ExecuteScalar();

                //Save the order items to the database
                query = "INSERT INTO OrderItems (OrderId, ProductId, Quantity) VALUES (@OrderId, @ProductId, @Quantity)";
                foreach (var cartItem in cartItems)
                {
                    using (var innerCommand = new SqlCommand(query, connection))
                    {
                        innerCommand.Parameters.AddWithValue("@OrderId", orderId);
                        innerCommand.Parameters.AddWithValue("@ProductId", cartItem.ProductId);
                        innerCommand.Parameters.AddWithValue("@Quantity", cartItem.Quantity);
                        innerCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        //Clear the cart
        ClearCart(userId);

        Console.WriteLine("Order placed successfully!");
    }
    private static List<Product> GetCartItems(int userId)
    {
        var cartItems = new List<Product>();

        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var query = "SELECT ProductId, Quantity FROM Product WHERE UserId = @UserId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var productId = (int)reader["ProductId"];
                        var quantity = (int)reader["Quantity"];
                        var cartItem = new Product(productId, quantity);
                        cartItems.Add(cartItem);
                    }
                }
            }
        }

        return cartItems;
    }

    private static void ClearCart(int userId)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var query = "DELETE FROM CartItems WHERE UserId = @UserId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Cart cleared.");
    }

    private static bool CheckProductInCart(int productId, int userId)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var query = "SELECT COUNT(*) FROM CartItems WHERE ProductId = @ProductId AND UserId = @UserId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@UserId", userId);
                var count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }

    private static void UpdateCartItemQuantity(int productId, int userId, int quantity)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var query = "UPDATE CartItems SET Quantity = Quantity + @Quantity WHERE ProductId = @ProductId AND UserId = @UserId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }
        }
    }

    private static void GetFilteredProducts(decimal minPrice, decimal maxPrice, string category, int year)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var query = "SELECT * FROM Products WHERE Price >= @MinPrice AND Price <= @MaxPrice";

            if (!string.IsNullOrEmpty(category))
            {
                query += " AND Category = @Category";
            }

            if (year > 0)
            {
                query += " AND Year = @Year";
            }

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MinPrice", minPrice);
                command.Parameters.AddWithValue("@MaxPrice", maxPrice);

                if (!string.IsNullOrEmpty(category))
                {
                    command.Parameters.AddWithValue("@Category", category);
                }
                if (year > 0)
                {
                    command.Parameters.AddWithValue("@Year", year);
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var productId = (int)reader["ProductId"];
                        var productName = (string)reader["ProductName"];
                        var productPrice = (decimal)reader["Price"];
                        var productCategory = (string)reader["Category"];
                        var productYear = (int)reader["Year"];

                        Console.WriteLine($"Product ID: {productId}");
                        Console.WriteLine($"Product Name: {productName}");
                        Console.WriteLine($"Price: {productPrice}");
                        Console.WriteLine($"Category: {productCategory}");
                        Console.WriteLine($"Year: {productYear}");
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}

