using System;
using Npgsql;

namespace GrowingStrongAPI.DataAccess
{
    public interface IDbConnectionFactory
    {
        public NpgsqlConnection CreateConnection(string connectionString);
    }
}
