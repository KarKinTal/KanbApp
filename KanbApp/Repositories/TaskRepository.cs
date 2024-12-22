using SQLite;
using KanbApp.Models;

namespace KanbApp.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly SQLiteAsyncConnection _db;

    public TaskRepository(SQLiteAsyncConnection db)
    {
        _db = db;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        _db.CreateTableAsync<Models.Task>().Wait();
    }

    public async Task<List<Models.Task>> GetTasksByColumnIdAsync(int columnId)
    {
        return await _db.Table<Models.Task>().Where(t => t.ColumnId == columnId).ToListAsync();
    }

    public async Task<List<User>> GetUsersForTableAsync(int tableId)
    {
        return await _db.QueryAsync<User>(
            "SELECT u.* FROM User u " +
            "JOIN TableUser tu ON u.Id = tu.UserId " +
            "WHERE tu.TableId = ?", tableId);
    }
}
