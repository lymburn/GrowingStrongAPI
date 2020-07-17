using System;
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

        public User GetById(int id)
        {
            try
            {
                using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
                {
                    string sql = $@"SELECT * FROM get_user_account_by_id({id})";

                    connection.Open();

                    User user = connection.Query<User>(sql).AsList().FirstOrDefault();

                    return user;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public User GetByEmailAddress(string emailAddress)
        {
            try
            {
                using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
                {
                    string sql = $@"SELECT * FROM get_user_account_by_email('{emailAddress}')";

                    connection.Open();

                    User user = connection.Query<User>(sql).AsList().FirstOrDefault();
                    return user;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Create(User user)
        {
            try
            {
                using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
                {
                    int id;

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

                    return id;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
