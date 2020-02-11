using System;
using System.Collections.Generic;
using Dapper;
using GrowingStrongAPI.Helpers;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.Helpers.Schemas;
using System.Linq;

namespace GrowingStrongAPI.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private IDbConnectionFactory _dbConnectionFactory;

        public UserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public User GetById(int id)
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConnectionHelper.ConnectionString))
            {
                string sql = $@"SELECT * FROM {UserSchema.Table}
                                WHERE {UserSchema.Columns.Id} = {id}";

                connection.Open();

                User user = connection.Query<User>(sql).AsList().FirstOrDefault();

                return user;
            }
        }

        public User GetByUsername(string username)
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConnectionHelper.ConnectionString))
            {
                string sql = $@"SELECT * FROM {UserSchema.Table}
                                WHERE {UserSchema.Columns.Username} = '{username}'";

                connection.Open();

                User user = connection.Query<User>(sql).AsList().FirstOrDefault();

                return user;
            }
        }

        public User Create(User user)
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConnectionHelper.ConnectionString))
            {
                string sql = $@"INSERT INTO {UserSchema.Table}({UserSchema.Columns.Username}, {UserSchema.Columns.EmailAddress}, {UserSchema.Columns.FirstName}, {UserSchema.Columns.LastName})
                                VALUES ({user.Username}, {user.EmailAddress}, {user.FirstName}, {user.LastName})";

                connection.Open();
                connection.Execute(sql);
            }

            return user;
        }
    }
}
