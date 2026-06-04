using Microsoft.Data.SqlClient;

namespace hospital.Data
{
    // Factory class responsible for creating SQL Server database connections
    public class SqlConnectionFactory
    {
        // Stores the connection string retrieved from configuration
        private readonly string _connectionString;

        // Constructor used to read the connection string from appsettings.json
        public SqlConnectionFactory(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // Creates and returns a new SQL connection object
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}