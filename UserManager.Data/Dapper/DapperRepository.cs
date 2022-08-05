using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace UserManager.Data.Dapper
{
    public class DapperRepository : 
        IUserRepository
    {
        private readonly IDatabaseConnection _connection;

        public DapperRepository(IDatabaseConnection connection)
        {
            _connection = connection;
        }
        
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            string query = 
                "SELECT * FROM Users";
            
            using (IDbConnection database = _connection.Connection())
            {
                var result = await database.QueryAsync<User>(query);
                return result.ToList();
            }
        }

        public async Task CreateAsync(User user)
        {
            string query = 
                "INSERT INTO Users " + 
                "VALUES(@FirstName, @LastName, @BirthDate, @Email, @PhoneNumber, @Info)";
            
            using (IDbConnection database = _connection.Connection())
            {
                await database.ExecuteAsync(query, user);
            }
        }

        public async Task<User> GetAsync(int id)
        {
            string query = 
                "SELECT * FROM Users WHERE Id = @id";
            
            using (IDbConnection database = _connection.Connection())
            {
                var result = await database.QueryAsync<User>(query, new { id });
                return result.FirstOrDefault();
            }
        }

        public async Task UpdateAsync(User user)
        {
            var query =
                "UPDATE Users " + 
                "SET FirstName = @FirstName, " + 
                "LastName = @LastName, " + 
                "BirthDate = @BirthDate, " + 
                "Email = @Email, " + 
                "PhoneNumber = @PhoneNumber, " + 
                "Info = @Info " + 
                "WHERE Id = @Id";

            using (IDbConnection database = _connection.Connection())
            {
                await database.ExecuteAsync(query, user);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var query =
                "DELETE FROM Users WHERE Id = @id";

            using (IDbConnection database = _connection.Connection())
            {
                await database.ExecuteAsync(query, new { id });
            }
        }
    }
}