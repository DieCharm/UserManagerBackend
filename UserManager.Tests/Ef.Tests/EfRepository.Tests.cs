using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManager.Data;
using UserManager.Data.EntityFramework;
using Xunit;

namespace UserManager.Tests.Ef.Tests
{
    public class EfRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_ReturnsCorrectData()
        {
            IUserRepository repository = await GetRepository();
            var actual = await repository.GetAllAsync();
            Assert.Equal(TestData.Users, actual);
        }
        
        [Fact]
        public async Task GetAsync_ReturnsCorrectUser()
        {
            IUserRepository repository = await GetRepository();
            var expected = TestData.Users.FirstOrDefault(user => user.Id == 3);
            var actual = await repository.GetAsync(3);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CreateAsync_AddsUser()
        {
            IUserRepository repository = await GetRepository();
            var toAdd = new User()
            {
                FirstName = "Recently",
                LastName = "Added",
                BirthDate = new DateTime(2000, 10, 10),
                Email = "somemail3@gmail.com",
                PhoneNumber = "+3807777773",
                Info = "Personal info..."
            };
            await repository.CreateAsync(toAdd);
            var expected = await repository.GetAsync(4);
            Assert.Equal(expected, toAdd);
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
            var expected = TestData.Users.Where(user => user.Id != 3);
            var actual = await repository.GetAllAsync();
            Assert.Equal(expected, actual);
        }

        private async Task<EfRepository> GetRepository()
        {
            string dbName = $"Database_{DateTime.Now.ToFileTimeUtc()}";
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            var context = new UserContext(options);
            await SeedData(context);
            return new EfRepository(context);
        }

        private async Task SeedData(UserContext context)
        {
            await context.Users.AddRangeAsync(TestData.Users);
            await context.SaveChangesAsync();
        }
    }
}