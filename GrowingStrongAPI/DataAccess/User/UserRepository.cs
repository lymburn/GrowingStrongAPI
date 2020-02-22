﻿using System;
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

        public IEnumerable<User> GetAll()
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConnectionHelper.ConnectionString))
            {
                string sql = $@"SELECT * FROM {UserSchema.Table}";

                connection.Open();

                IEnumerable<User> users = connection.Query<User>(sql);

                return users;
            }
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

        public User GetByEmailAddress(string emailAddress)
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConnectionHelper.ConnectionString))
            {
                string sql = $@"SELECT * FROM {UserSchema.Table}
                                WHERE {UserSchema.Columns.EmailAddress} = '{emailAddress}'";

                connection.Open();

                User user = connection.Query<User>(sql).AsList().FirstOrDefault();

                return user;
            }
        }

        public User Create(User user)
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConnectionHelper.ConnectionString))
            {
                string sql = $@"INSERT INTO {UserSchema.Table}({UserSchema.Columns.EmailAddress})
                                VALUES ({user.EmailAddress}";

                connection.Open();

                connection.Execute(sql);
            }

            return user;
        }
    }
}
