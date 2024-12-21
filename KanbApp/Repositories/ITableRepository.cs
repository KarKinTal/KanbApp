using KanbApp.Models;
using Task = System.Threading.Tasks.Task;

namespace KanbApp.Repositories;

public interface ITableRepository
{
    Task<List<Table>> GetTablesForUserAsync(int userId);
    Task AddTableAsync(Table table);
    Task<Table> GetTableByIdAsync(int tableId);
    Task<Table> GetTableByCodeAsync(string tableCode);
    Task<bool> DeleteTableAsync(int tableId);
    Task<bool> AddUserToTableAsync(int tableId, int userId);
    Task<bool> RemoveUserFromTableAsync(int tableId, int userId);
    Task<bool> UpdateTableAsync(Table table);
    Task<bool> DoesCodeExistAsync(string code);
}
