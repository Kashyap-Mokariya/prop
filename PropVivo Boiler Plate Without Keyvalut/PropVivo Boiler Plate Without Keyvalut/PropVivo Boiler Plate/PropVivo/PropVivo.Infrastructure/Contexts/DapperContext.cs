using Microsoft.Extensions.Configuration;
using PropVivo.Domain.Enums;
using System.Data;
using System.Data.SqlClient;

namespace PropVivo.Infrastructure.Contexts
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectionString = "Insert connection string";
        }

        public SqlConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}