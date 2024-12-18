using System.Security.Cryptography;
using System.Text;
using KanbApp.Repositories;
using KanbApp.Models;

namespace KanbApp.Services;

public class UserService
{
    private readonly IUserRepository _userRepository; // Interfejs zarządzania użytkownikami.
    private readonly ITableRepository _tableRepository;

    public UserService(IUserRepository userRepository, ITableRepository tableRepository)
    {
        _userRepository = userRepository;
        _tableRepository = tableRepository;
    }

    // Hashowanie hasła przy użyciu SHA256
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public async Task<bool> RegisterUserAsync(string email, string name, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name))
            throw new ArgumentException("Email, name, and password cannot be empty.");

        var hashedPassword = HashPassword(password);

        return await _userRepository.CreateUserAsync(email, name, hashedPassword);
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
            return false;

        if (user.PasswordHash != HashPassword(password))
        {
            return false;
        }

        await SecureStorage.SetAsync("LoggedInUserId", user.Id.ToString());

        return true;
    }

    public async Task<bool> ChangePasswordAsync(string email, string newPassword, string confirmPassword)
    {
        if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            throw new ArgumentException("Password fields cannot be empty.");

        if (newPassword != confirmPassword)
            throw new ArgumentException("Passwords do not match.");

        var hashedPassword = HashPassword(newPassword);

        return await _userRepository.UpdateUserPasswordAsync(email, hashedPassword);
    }

    public async Task<bool> UpdateUserDataAsync(string email, string newName, string newPassword)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newName))
            throw new ArgumentException("Email and name cannot be empty.");

        var hashedPassword = string.IsNullOrEmpty(newPassword) ? null : HashPassword(newPassword);

        return await _userRepository.UpdateUserDataAsync(email, newName, hashedPassword);
    }

    public async Task<List<User>> GetUsersWithAccessToTableAsync(int tableId)
    {
        return await _userRepository.GetUsersWithAccessToTableAsync(tableId);
    }

    public async Task<List<User>> GetUsersWithAccessToTaskAsync(int taskId)
    {
        return await _userRepository.GetUsersWithAccessToTaskAsync(taskId);
    }

    public async Task<List<Table>> GetTablesForLoggedInUserAsync()
    {
        var loggedInUser = await GetLoggedInUserAsync();
        if (loggedInUser == null)
        {
            return new List<Table>(); // Brak zalogowanego użytkownika
        }

        return await _tableRepository.GetTablesForUserAsync(loggedInUser.Id);
    }

    public async Task<User> GetLoggedInUserAsync()
    {
        var userIdString = await SecureStorage.GetAsync("LoggedInUserId");

        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            return null; // Brak zalogowanego użytkownika
        }

        return await _userRepository.GetUserByIdAsync(userId); // Pobierz użytkownika po ID
    }

    public async Task<bool> LogoutAsync(string email)
    {
        SecureStorage.Remove("LoggedInUserId");
        return true;
    }
}