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

    public async Task<bool> DeleteTaskAsync(int taskId)
    {
        var task = await _db.FindAsync<Models.Task>(taskId);
        if (task == null)
            return false;

        // Usuń powiązania użytkowników z zadaniem
        var taskUsers = await _db.Table<TaskUser>().Where(tu => tu.TaskId == taskId).ToListAsync();
        foreach (var taskUser in taskUsers)
        {
            await _db.DeleteAsync(taskUser);
        }

        // Usuń samo zadanie
        var result = await _db.DeleteAsync(task);
        return result > 0;
    }
}
