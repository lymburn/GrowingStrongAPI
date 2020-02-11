using System;
using System.Collections.Generic;
using Dapper;
using GrowingStrongAPI.Helpers;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.Helpers.Schemas;

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
                string sql = $@"INSERT INTO {UserSchema.Table}({UserSchema.Columns.Username}, {UserSchema.Columns.EmailAddress}, {UserSchema.Columns.FirstName}, {UserSchema.Columns.LastName})
                                VALUES ({user.Username}, {user.EmailAddress}, {user.FirstName}, {user.LastName})";

                connection.Open();
                connection.Execute(sql);
            }
        }

        public List<User> FindUserByUsername(string username)
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConnectionHelper.ConnectionString))
            {
                string sql = $@"SELECT * FROM {UserSchema.Table}
                                WHERE {UserSchema.Columns.Username} = '{username}'";

                connection.Open();
                return connection.Query<User>(sql).AsList();
            }
        }

        public List<User> FindUserById(int id)
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConnectionHelper.ConnectionString))
            {
                string sql = $@"SELECT * FROM {UserSchema.Table}
                                WHERE {UserSchema.Columns.Id} = {id}";

                connection.Open();
                return connection.Query<User>(sql).AsList();
            }
        }
    }
}
