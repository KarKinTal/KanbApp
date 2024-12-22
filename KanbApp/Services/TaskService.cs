using KanbApp.Models;
using KanbApp.Repositories;
using SQLite;
using Task = System.Threading.Tasks.Task;

namespace KanbApp.Services;

public class TaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly SQLiteAsyncConnection _db;

    public TaskService(ITaskRepository taskRepository, SQLiteAsyncConnection db)
    {
        _taskRepository = taskRepository;
        _db = db;
    }

    public async Task<List<Models.Task>> GetTasksByColumnIdAsync(int columnId)
    {
        return await _taskRepository.GetTasksByColumnIdAsync(columnId);
    }

    public async Task<bool> DoesColumnContainTasksForUserAsync(int columnId, int userId)
    {
        var tasks = await _taskRepository.GetTasksByColumnIdAsync(columnId);
        return tasks.Any(t => t.OwnerId == userId);
    }

    public async Task<int> AddTaskAsync(Models.Task task)
    {
        await _db.InsertAsync(task);
        return task.Id;
    }

    public async Task AssignUsersToTask(int taskId, List<int> userIds)
    {
        foreach (var userId in userIds)
        {
            var taskUser = new TaskUser
            {
                TaskId = taskId,
                UserId = userId
            };
            await _db.InsertAsync(taskUser);
        }
    }
}