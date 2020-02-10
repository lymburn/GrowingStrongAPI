using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Dapper;
using Npgsql;
using GrowingStrongAPI.Helpers;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Services;

namespace GrowingStrongAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IEnumerable<string> GetAll()
        {
            Console.WriteLine("Fetching all users");
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Console.WriteLine($"Fetching user with id {id}");
            User user = _userService.GetById(id);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public void Register([FromBody]User userDetails)
        {
            Console.WriteLine($"Creating new user with details {userDetails.Id}, {userDetails.FirstName}, {userDetails.LastName}");
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
