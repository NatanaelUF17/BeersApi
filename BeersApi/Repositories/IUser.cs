using System.Collections.Generic;
using System.Threading.Tasks;
using BeersApi.Models;

namespace BeersApi.Repositories
{
    public interface IUser
    {
        Task Save(User user);
        Task Update(int id);
        Task<bool> Delete(int id);
        Task<User> GetUser(int id);
        Task<List<User>> GetAllUsers();
    }
}