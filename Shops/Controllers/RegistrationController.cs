using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using Shops.Models;
using System.Configuration;
using Npgsql;
using System.Data;


namespace Shops.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IConfiguration? configuration;

        public RegistrationController()
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("Registration")]
        public string registration(User registration)
        {
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("").ToString());
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO User(userName, email, password, avatar, roleName) VALUES('"+registration.userName+ "','"+registration.email+ "','"+registration.password+ "','"+registration.avatar+ "','"+registration.roleName+ "',)", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if(i > 0)
            {
                return "Data inserted";
            }
            else
            {
                return "errr";
            }
            return "";
        }
        [HttpPost]
        [Route("Login")]
        public string login(User registration)
        {
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("").ToString());
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT  *FROM User WHERE email = '"+registration.email+ "' AND password = '"+registration.password+ "' ",con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                return "Data Found";
            }
            else
            {
                return "Invaild User";
            }

        }
    }
}
