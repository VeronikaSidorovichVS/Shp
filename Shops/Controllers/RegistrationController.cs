using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shops.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace MyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("SignUp")]
        [SwaggerOperation("UserSignUp")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad Request")]
        public IActionResult SignUp([FromBody] UserSignUpModel model)
        {
            try
            {
                // Проверка уникальности email
                if (_dbContext.Users.Any(u => u.Email == model.Email))
                {
                    // Если email уже используется, выводим сообщение об ошибке
                    ModelState.AddModelError("Email", "Пользователь с таким email уже существует");
                    return BadRequest();
                }

                // Создание нового пользователя
                var user = new User { Email = model.Email, Password = model.Password, userName = model.userName };
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                
                //SignIn(user);

                return Ok(); 
            }
            catch (DbUpdateException ex)
            {
                // Ошибка при сохранении пользователя в базе данных
                ModelState.AddModelError("SaveError", "Произошла ошибка при сохранении пользователя");
                return BadRequest();
            }
            catch (Exception ex)
            {
                // Обработка других исключений
                return BadRequest();
            }
        }

        public class UserSignUpModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string userName { get; set; }

        }

        [HttpPost]
        [Route("SignIn")]
        [SwaggerOperation("UserSignIn")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad Request")]
        public IActionResult SignIn(string email, string password)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
                if (user == null)
                {
                    // Если введены неверные данные, выводим сообщение об ошибке
                    ModelState.AddModelError("", "Неверный email или пароль");
                    return BadRequest();
                }

                // Вход пользователя
                SignIn(User);

                // Перенаправление на сайт с кодом состояния 200
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                // Обработка исключений
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddToCart")]
        [SwaggerOperation("AddToCart")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad Request")]
        public IActionResult AddToCart(int productId)
        {
            try
            {
                var user = GetCurrentUser();
                var product = _dbContext.Products.Find(productId);

                // Добавление товара в корзину пользователя
                user.CartItems.Add(new CartItem { Product = product });

                _dbContext.SaveChanges();

                // Перенаправление на страницу товара или корзины с кодом состояния 200
                return StatusCode(StatusCodes.Status200OK, new { id = productId });
            }
            catch (Exception ex)
            {
                // Обработка исключений
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("RemoveFromCart")]
        [SwaggerOperation("RemoveFromCart")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad Request")]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            try
            {
                var user = GetCurrentUser();
                var cartItem = user.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

                if (cartItem != null)
                {
                    // Удаление товара из корзины пользователя
                    user.CartItems.Remove(cartItem);
                    _dbContext.SaveChanges();
                }

                // Перенаправление на страницу корзины с кодом состояния 200
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                // Обработка исключений
                return BadRequest();
            }
        }
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [Route("api/users/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    // Удаление пользователя из базы данных
                    _dbContext.Users.Remove(user);
                    _dbContext.SaveChanges();
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                // Обработка исключений
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [Route("api/orders")]
        public IActionResult PlaceOrder(OrderData orderData)
        {
            try
            {
                var user = GetCurrentUser();
                // Добавление товаров из корзины в заказ
                foreach (var cartItem in user.CartItems)
                {
                    var orderItem = new OrderItem { ProductId = cartItem.Product, Quantity = cartItem.Quantity };
                    //orderData.Items.Add(orderItem);
                }

                // Создание нового заказа
                var order = new Order { UserId = user.Id, OrderData = orderData };
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();

                // Очистка корзины пользователя
                user.CartItems.Clear();
                _dbContext.SaveChanges();

                // Возвращение успешного результата кода 200
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                // Обработка исключений
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetFilteredProducts")]
        [SwaggerOperation("GetFilteredProducts")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad Request")]
        public IActionResult GetFilteredProducts(string category)
        {
            try
            {
                var products = _dbContext.Products.Where(p => p.Category == category).ToList();

                // Возвращение успешного результата кода 200 с отфильтрованными продуктами
                return StatusCode(StatusCodes.Status200OK, products);
            }
            catch (Exception ex)
            {
                // Обработка исключений
                return BadRequest();
            }
        }


        private User GetCurrentUser()
        {
           
            return null;
        }
    }
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
        public object Quantity { get; internal set; }
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
        public object ProductId { get; internal set; }
        public object Quantity { get; internal set; }
    }

    public class OrderData
    {
        internal object Items;

        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class ApplicationDbContext : DbContext
    {
        

  

   
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}

