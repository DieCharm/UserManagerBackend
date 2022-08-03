using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManager.Data
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task CreateAsync(User user);
        Task<User> GetAsync(int id);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}