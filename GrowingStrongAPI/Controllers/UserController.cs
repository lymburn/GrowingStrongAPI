using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Dapper;
using Npgsql;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.Services;

namespace GrowingStrongAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            Console.WriteLine("Fetching all users");
            List<User> users = _userService.GetAll().AsList();

            return Ok(users);
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
        public IActionResult Register([FromBody]RegistrationModel registrationModel)
        {
            //Console.WriteLine($"Creating new user with details {userDetails.Id}, {userDetails.FirstName}, {userDetails.LastName}");
            User user = _mapper.Map<User>(registrationModel);

            _userService.Create(user, registrationModel.Password);
            return Ok();
            
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
