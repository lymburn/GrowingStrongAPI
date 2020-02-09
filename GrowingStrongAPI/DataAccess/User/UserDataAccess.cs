using System;
using System.Collections.Generic;
using Dapper;
using GrowingStrongAPI.Helpers;
using GrowingStrongAPI.Models;


namespace GrowingStrongAPI.DataAccess
{
    public class UserDataAccess : IUserDataAccess
    {
        private IDbConnectionFactory _dbConnectionFactory;

        public UserDataAccess(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public void InsertUser(User user)
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConnectionHelper.ConnectionString))
            {
                string sql = $"INSERT INTO useraccount(username, email_address, first_name, last_name) VALUES ({user.Username}, {user.EmailAddress}, {user.FirstName}, {user.LastName})";
                connection.Open();
                connection.Execute(sql);
            }
        }

        public List<User> FindUserByUsername(string username)
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConnectionHelper.ConnectionString))
            {
                string sql = $"SELECT * FROM useraccount WHERE username = '{username}'";
                connection.Open();
                return connection.Query<User>(sql).AsList();
            }
        }

        public List<User> FindUserById(int id)
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConnectionHelper.ConnectionString))
            {
                string sql = $"SELECT * FROM useraccount WHERE id = {id}";
                connection.Open();
                return connection.Query<User>(sql).AsList();
            }
        }
    }
}
