﻿using System;
using System.Collections.Generic;
using Npgsql;
using Dapper;
using GrowingStrongAPI.Helpers;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.Helpers.Schemas;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace GrowingStrongAPI.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger _logger;

        public UserRepository(IDbConnectionFactory dbConnectionFactory,
                              ILogger<IUserRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public IEnumerable<User> GetAll()
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
            {
                string sql = $@"SELECT * FROM {UserSchema.Table}";

                connection.Open();

                IEnumerable<User> users = connection.Query<User>(sql);

                return users;
            }
        }

        public User GetById(int id)
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
            {
                //string sql = $@"SELECT * FROM {UserSchema.Table}
                //                WHERE {UserSchema.Columns.Id} = {id}";

                string sql = $@"SELECT * FROM get_user_account_by_id({id})";

                connection.Open();

                User user = connection.Query<User>(sql).AsList().FirstOrDefault();

                return user;
            }
        }

        public User GetByEmailAddress(string emailAddress)
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
            {
                string sql = $@"SELECT * FROM get_user_account_by_email('{emailAddress}')";

                connection.Open();

                User user = connection.Query<User>(sql).AsList().FirstOrDefault();
                return user;
            }
        }

        public int Create(User user)
        {
            using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
            {
                int id;

                try
                {
                    string sql = $@"select * from create_user_account(@EmailAddress, @PasswordHash, @PasswordSalt)";

                    NpgsqlCommand command = new NpgsqlCommand(sql, connection);

                    NpgsqlParameter emailParam = new NpgsqlParameter("@EmailAddress", NpgsqlTypes.NpgsqlDbType.Varchar);

                    NpgsqlParameter passwordHashParam = new NpgsqlParameter("@PasswordHash", NpgsqlTypes.NpgsqlDbType.Bytea);

                    NpgsqlParameter passwordSaltParam = new NpgsqlParameter("@PasswordSalt", NpgsqlTypes.NpgsqlDbType.Bytea);

                    emailParam.Value = user.EmailAddress;

                    passwordHashParam.Value = user.PasswordHash;

                    passwordSaltParam.Value = user.PasswordSalt;

                    NpgsqlParameter[] parameters = new NpgsqlParameter[] { emailParam, passwordHashParam, passwordSaltParam, };

                    command.Parameters.AddRange(parameters);

                    connection.Open();

                    id = Int32.Parse(command.ExecuteScalar().ToString());
                }
                catch (Exception e)
                {
                    throw e;
                }

                return id;
            }
        }
    }
}
