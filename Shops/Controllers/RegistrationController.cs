using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shops;
using Shops.Models;
using Swashbuckle.AspNetCore.Annotations;
using static Shops.Models.modelcs;
using User = Shops.Models.modelcs.User;

namespace MyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class AccountController : ControllerBase
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
                if (_dbContext.Users.Any(u => u.email == model.Email))
                {
                    // Если email уже используется, выводим сообщение об ошибке
                    ModelState.AddModelError("Email", "Пользователь с таким email уже существует");
                    return BadRequest();
                }

                // Создание нового пользователя
                var user = new User { Email = model.Email, Password = model.Password, userName = model.userName };
                //_dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                //SignIn(user);

                return Ok();
            }
            catch (DbUpdateException ex)
            {
                // Ошибка при сохранении пользователя в базе данных
                ModelState.AddModelError("SaveError", "Произошла ошибка при сохранении пользователя");
                return BadRequest("error saving db");
            }
            catch (Exception ex)
            {
                // Обработка других исключений
                return BadRequest(ex.Message);
            }
        }

        //public class UserSignUpModel
        //{
        //    public string Email { get; set; }
        //    public string Password { get; set; }
        //    public string userName { get; set; }

        //}

        [HttpPost]
        [Route("SignIn")]
        [SwaggerOperation("UserSignIn")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad Request")]
        public IActionResult SignIn(string email, string password)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.email == email && u.password == password);
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
                return BadRequest(ex.Message);
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
                //user.CartItems.Add(new CartItem { Product = products });

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
                var user = _dbContext.Users.FirstOrDefault(u => u.userId == userId);
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
                    var orderItem = new OrderItem { ProductId = cartItem.Id, Quantity = cartItem.Quantity };
                    //orderData.Items.Add(orderItem);
                }

                // Создание нового заказа
                var order = new Order { UserId = user.Id, OrderData = orderData };
                _dbContext.orders.Add(order);
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
        public IActionResult GetFilteredProducts(string brand)
        {
            try
            {
                var products = _dbContext.Products.Where(p => p.Brand == brand).ToList();

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
    

}

