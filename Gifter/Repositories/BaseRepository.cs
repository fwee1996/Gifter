using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Gifter.Repositories
{
    //abstract: BaseRepository class cannot be directly instantiated, but can ONLY be used by inheritance.
    public abstract class BaseRepository
    {
        private readonly string _connectionString;

        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //protected:  available to child classes, but inaccessible to any other code.
        protected SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }
    }
}