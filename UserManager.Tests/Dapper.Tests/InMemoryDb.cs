using System.Data;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using UserManager.Data;

namespace UserManager.Tests.Dapper.Tests
{
    public class InMemoryDb
    {
        private readonly OrmLiteConnectionFactory _dbFactory;

        public InMemoryDb()
        {
            _dbFactory = new OrmLiteConnectionFactory(
                ":memory:", 
                SqliteOrmLiteDialectProvider.Instance);
        }

        public async Task SeedData()
        {
            using (var db = OpenConnection())
            {
                db.CreateTableIfNotExists<User>();
                //db.AlterTable<User>("ADD PRIMARY KEY(Id)");
                
                foreach (User user in TestData.Users)
                {
                    await db.InsertAsync(user);
                }

                if (await db.TableExistsAsync("User"))
                {
                    db.AlterTable<User>("RENAME TO Users");
                }
            }
        }

        public IDbConnection OpenConnection()
        {
            return _dbFactory.OpenDbConnection();
        }
    }
}