using Truck_Visit_Management.Entities;

namespace Truck_Visit_Management.Repositories
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        User GetById(int id);
        Task AddUserAsync(User user);
        Task<bool> UsernameExistsAsync(string username);
    }
}
