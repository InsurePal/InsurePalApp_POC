using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Neolab.Code.DAL
{
    public class DbAccessObject
    {
        public DbAccessObject()
        {
            _connectionString = "DefaultConnection";
            _dbPrivate = new SqlConnection(ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString);
        }
        public DbAccessObject(string ConnectionString)
        {
            _connectionString = ConnectionString;
            _dbPrivate = new SqlConnection(ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString);
        }
        private string _connectionString { get; set; }
        private IDbConnection _dbPrivate { get; set; }
        public IDbConnection _db { get { return _dbPrivate; } }
    }
}