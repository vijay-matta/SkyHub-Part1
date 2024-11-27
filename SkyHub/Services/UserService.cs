using SkyHub.Data;
using SkyHub.Models.Roles;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyHub.Services
{
    

    public class UserService : IUserService
    {
        private readonly SkyHubDbContext _context;

        public UserService(SkyHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await _context.Users.Include(u => u.Customer)
                                       .Include(u => u.FlightOwner)
                                       .Include(u => u.Admin)
                                       .ToListAsync();
        }

        public async Task<Users> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                                 .Include(u => u.Customer)
                                 .Include(u => u.FlightOwner)
                                 .Include(u => u.Admin)
                                 .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task UpdateUserAsync(int userId, Users updatedUser)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                user.UserName = updatedUser.UserName;
                user.Email = updatedUser.Email;
                user.RoleType = updatedUser.RoleType;
                user.DateJoined = updatedUser.DateJoined;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
