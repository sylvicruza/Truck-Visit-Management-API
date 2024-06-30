using Microsoft.EntityFrameworkCore;
using Truck_Visit_Management.Data;
using Truck_Visit_Management.Entities;

namespace Truck_Visit_Management.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TruckVisitDbContext _context;

        public UserRepository(TruckVisitDbContext context)
        {
            _context = context;
        }

        public User GetByUsername(string username)
        {
            return _context.User.SingleOrDefault(u => u.Username == username);
        }

        public User GetById(int id)
        {
            return _context.User.SingleOrDefault(u => u.Id == id);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.User.AnyAsync(u => u.Username == username);
        }

    }

}
