using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using UserManager.Data;
using UserManager.Data.Dapper;
using Xunit;

namespace UserManager.Tests.Dapper.Tests
{
    public class DapperRepositoryTests
    {
        private readonly OrmLiteConnectionFactory _dbFactory;

        public DapperRepositoryTests()
        {
            _dbFactory = new OrmLiteConnectionFactory(
                ":memory:", SqliteOrmLiteDialectProvider.Instance);
        }
        
        [Fact]
        public async Task GetAsync_ReturnsCorrectUser()
        {
            IUserRepository repository = await GetRepository();
            var expected = JsonConvert.SerializeObject(
                TestData.Users.FirstOrDefault(user => user.Id == 3));
            var actual = JsonConvert.SerializeObject(
                await repository.GetAsync(3));
            Assert.Equal(expected,actual);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsCorrectData()
        {
            IUserRepository repository = await GetRepository();
            var expected = JsonConvert.SerializeObject(
                TestData.Users);
            var actual = JsonConvert.SerializeObject(
                await repository.GetAllAsync());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CreateAsync_AddsUser()
        {
            IUserRepository repository = await GetRepository();
            User toAdd = new User()
            {
                Id = 4,
                FirstName = "Recently",
                LastName = "Added",
                BirthDate = new DateTime(2000, 10, 10),
                Email = "somemail3@gmail.com",
                PhoneNumber = "+3807777773",
                Info = "Personal info..."
            };
            await repository.CreateAsync(toAdd);
            var expected = JsonConvert.SerializeObject(
                await repository.GetAsync(4));
            var actual = JsonConvert.SerializeObject(toAdd);
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task UpdateAsync_UpdatesUser()
        {
            IUserRepository repository = await GetRepository();
            string newLastName = "Updated";
            User toUpdate = await repository.GetAsync(3);
            toUpdate.LastName = newLastName;
            await repository.UpdateAsync(toUpdate);
            var updated = await repository.GetAsync(3);
            Assert.Equal(newLastName, updated.LastName);
        }

        [Fact]
        public async Task DeleteAsync_RemovesUser()
        {
            IUserRepository repository = await GetRepository();
            await repository.DeleteAsync(3);
            var expected = JsonConvert.SerializeObject(
                TestData.Users.Where(user => user.Id != 3));
            var actual = JsonConvert.SerializeObject(
                await repository.GetAllAsync());
            Assert.Equal(expected, actual);
        }

        private async Task<DapperRepository> GetRepository()
        {
            InMemoryDb db = new InMemoryDb();
            await db.SeedData();
            var connectionMock = new Mock<IDatabaseConnection>();
            connectionMock.Setup(c => c.Connection())
                .Returns(db.OpenConnection);
            DapperRepository repository = new DapperRepository(connectionMock.Object);
            return repository;
        }
    }
}