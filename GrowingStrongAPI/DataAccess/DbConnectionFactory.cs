using System;
using Npgsql;

namespace GrowingStrongAPI.DataAccess
{

    public class DbConnectionFactory : IDbConnectionFactory
    {
        public DbConnectionFactory()
        {
        }

        public NpgsqlConnection CreateConnection (string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
