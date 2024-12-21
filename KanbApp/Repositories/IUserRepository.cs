using KanbApp.Models;
namespace KanbApp.Repositories;

public interface IUserRepository
{
    Task<bool> CreateUserAsync(string email, string name, string passwordHash);
    Task<bool> UpdateUserPasswordAsync(string email, string passwordHash);
    Task<bool> UpdateUserDataAsync(string email, string newName, string? passwordHash);
    Task<List<User>> GetUsersWithAccessToTableAsync(int tableId);
    Task<List<User>> GetUsersWithAccessToTaskAsync(int taskId);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByIdAsync(int userId);
    Task<User?> GetLoggedInUserAsync();
}
