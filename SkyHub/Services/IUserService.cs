using SkyHub.Models.Roles;

namespace SkyHub.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Users>> GetAllUsersAsync();
        Task<Users> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(int userId, Users updatedUser);
        Task DeleteUserAsync(int userId);
    }
}
