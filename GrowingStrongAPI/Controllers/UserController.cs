using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Dapper;
using Npgsql;
using GrowingStrongAPI.DataAccess;
using GrowingStrongAPI.Models;

namespace GrowingStrongAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        [HttpGet]
        public IEnumerable<string> FetchAllUsers()
        {
            Console.WriteLine("Fetching all users");
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string FetchUserById(int id)
        {
            Console.WriteLine($"Fetching user with id {id}");
            return "value";
        }

        [HttpPost]
        public void CreateUser([FromBody]User userDetails)
        {
            Console.WriteLine($"Creating new user with details {userDetails.Id}, {userDetails.FirstName}, {userDetails.LastName}");
            using (var connection = new NpgsqlConnection(ConnectionHelper.ConnectionString))
            {
                connection.Open();
                connection.Execute("INSERT into UserAccount (id, fjwtirst_name, last_name) VALUES ('Eugene', 'Lu')");
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
