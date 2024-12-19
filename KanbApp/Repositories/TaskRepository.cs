using SQLite;
using KanbApp.Models;

namespace KanbApp.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly SQLiteAsyncConnection _db;

    public TaskRepository(SQLiteAsyncConnection db)
    {
        _db = db;
    }

    public async Task<List<Models.Task>> GetTasksByColumnIdAsync(int columnId)
    {
        return await _db.Table<Models.Task>().Where(t => t.ColumnId == columnId).ToListAsync();
    }
}
