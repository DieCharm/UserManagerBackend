using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UserManager.Data.Dapper;

namespace UserManager
{
    public class SqlServerDatabaseConnection : 
        IDatabaseConnection
    {
        private readonly IConfiguration _configuration;

        public SqlServerDatabaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection Connection()
        {
            return new SqlConnection(_configuration.GetConnectionString("Users"));
        }
    }
}