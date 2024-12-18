using KanbApp.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SQLiteAsyncConnection _db;

        public UserRepository(SQLiteAsyncConnection db)
        {
            _db = db;
            InitializeDatabase();
        }

        // Inicjalizacja tabeli User, jeśli nie istnieje
        private void InitializeDatabase()
        {
            _db.CreateTableAsync<User>().Wait();
        }

        // Tworzenie nowego użytkownika
        public async Task<bool> CreateUserAsync(string email, string name, string passwordHash)
        {
            // Sprawdzenie, czy użytkownik z takim emailem już istnieje
            var existingUser = await _db.Table<User>().FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return false; // Email już istnieje
            }

            // Dodanie nowego użytkownika
            var newUser = new User
            {
                Email = email,
                Name = name,
                PasswordHash = passwordHash
            };

            var result = await _db.InsertAsync(newUser);
            return result > 0; // Zwraca true, jeśli dodanie się powiodło
        }

        // Pobieranie użytkownika po emailu
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _db.Table<User>().FirstOrDefaultAsync(u => u.Email == email);
        }

        // Aktualizacja hasła użytkownika
        public async Task<bool> UpdateUserPasswordAsync(string email, string passwordHash)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null)
            {
                return false; // Użytkownik nie istnieje
            }

            user.PasswordHash = passwordHash;
            var result = await _db.UpdateAsync(user);
            return result > 0; // Zwraca true, jeśli aktualizacja się powiodła
        }

        // Aktualizacja danych użytkownika (tylko nazwy i opcjonalnie hasła)
        public async Task<bool> UpdateUserDataAsync(string email, string newName, string? passwordHash)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null)
            {
                return false; // Użytkownik nie istnieje
            }

            user.Name = newName;
            if (!string.IsNullOrEmpty(passwordHash))
            {
                user.PasswordHash = passwordHash;
            }

            var result = await _db.UpdateAsync(user);
            return result > 0; // Zwraca true, jeśli aktualizacja się powiodła
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _db.Table<User>().FirstOrDefaultAsync(u => u.Id == userId);
        }


        // Pobieranie użytkowników z dostępem do tabeli
        public async Task<List<User>> GetUsersWithAccessToTableAsync(int tableId)
        {
            // Pobranie powiązań z tabeli TableUser
            var tableUsers = await _db.Table<TableUser>().Where(tu => tu.TableId == tableId).ToListAsync();
            var userIds = tableUsers.Select(tu => tu.UserId).ToList();

            // Pobranie użytkowników na podstawie ID
            return await _db.Table<User>().Where(u => userIds.Contains(u.Id)).ToListAsync();
        }

        // Pobieranie użytkowników z dostępem do zadania
        public async Task<List<User>> GetUsersWithAccessToTaskAsync(int taskId)
        {
            // Pobranie powiązań z tabeli TaskUser
            var taskUsers = await _db.Table<TaskUser>().Where(tu => tu.TaskId == taskId).ToListAsync();
            var userIds = taskUsers.Select(tu => tu.UserId).ToList();

            // Pobranie użytkowników na podstawie ID
            return await _db.Table<User>().Where(u => userIds.Contains(u.Id)).ToListAsync();
        }
    }
}