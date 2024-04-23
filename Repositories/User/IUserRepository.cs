using System.Collections.Generic;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAndPassword(string email, string password);
        Task<User> GetByIdAsync(int id);
        Task<List<User>?> GetAllUsers();
        Task<User> GetByIdUser(int id);
        Task<bool> AddUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(int id);
        Task<bool> UserExistsUser(int id);
    }

}
