using KanbApp.Models;
using KanbApp.Repositories;
using SQLite;
using Task = System.Threading.Tasks.Task;

namespace KanbApp.Services;

public class TaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITableRepository _tableRepository;
    private readonly SQLiteAsyncConnection _db;

    public TaskService(ITaskRepository taskRepository, ITableRepository tableRepository, SQLiteAsyncConnection db)
    {
        _taskRepository = taskRepository;
        _tableRepository = tableRepository;
        _db = db;
    }
    public async Task<Table?> GetTableByCodeAsync(string tableCode)
    {
        if (string.IsNullOrWhiteSpace(tableCode))
            return null;

        return await _tableRepository.GetTableByCodeAsync(tableCode);
    }

    public async Task<List<Models.Task>> GetTasksByColumnIdAsync(int columnId)
    {
        return await _taskRepository.GetTasksByColumnIdAsync(columnId);
    }

    public async Task<List<Models.Task>> GetTasksByColumnIdWithUsersAsync(int columnId)
    {
        var tasks = await _db.Table<Models.Task>().Where(t => t.ColumnId == columnId).ToListAsync();

        foreach (var task in tasks)
        {
            var taskUsers = await _db.Table<TaskUser>()
                                     .Where(tu => tu.TaskId == task.Id)
                                     .ToListAsync();

            // Pobieranie nazw użytkowników przypisanych do zadania
            var userIds = taskUsers.Select(tu => tu.UserId).ToList();
            var users = await _db.Table<User>()
                                 .Where(u => userIds.Contains(u.Id))
                                 .ToListAsync();

            // Łączenie nazw użytkowników jako string
            task.UsersNames += $"{string.Join(", ", users.Select(u => u.Name))}";
        }

        return tasks;
    }

    public async Task<bool> DoesColumnContainTasksForUserAsync(int columnId, int userId)
    {
        var tasks = await _taskRepository.GetTasksByColumnIdAsync(columnId);
        return tasks.Any(t => t.OwnerId == userId);
    }

    public async Task<bool> UpdateTaskAsync(Models.Task task)
    {
        var existingTask = await _db.FindAsync<Models.Task>(task.Id);
        if (existingTask == null) return false;

        existingTask.ColumnId = task.ColumnId;
        existingTask.Name = task.Name;
        existingTask.Description = task.Description;
        existingTask.Date = task.Date;

        await _db.UpdateAsync(existingTask);
        return true;
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

        var assignedUsers = await _db.Table<TaskUser>().Where(tu => tu.TaskId == taskId).ToListAsync();
        System.Diagnostics.Debug.WriteLine($"Task {taskId} assigned to users: {string.Join(", ", assignedUsers.Select(tu => tu.UserId))}");
    }

    public async Task<Models.Task> GetTaskByIdAsync(int taskId)
    {
        return await _db.FindAsync<Models.Task>(taskId);
    }

    public async Task<List<User>> GetUsersForTaskAsync(int taskId)
    {
        var taskUsers = await _db.Table<TaskUser>()
                                 .Where(tu => tu.TaskId == taskId)
                                 .ToListAsync();

        var userIds = taskUsers.Select(tu => tu.UserId).ToList();
        return await _db.Table<User>().Where(u => userIds.Contains(u.Id)).ToListAsync();
    }

    public async Task UpdateTaskUsersAsync(int taskId, List<int> userIds)
    {
        var existingUsers = await _db.Table<TaskUser>()
                                     .Where(tu => tu.TaskId == taskId)
                                     .ToListAsync();

        // Usuń użytkowników, którzy nie są już przypisani
        foreach (var user in existingUsers)
        {
            if (!userIds.Contains(user.UserId))
            {
                await _db.DeleteAsync(user);
            }
        }

        // Dodaj nowych użytkowników
        foreach (var userId in userIds)
        {
            if (!existingUsers.Any(u => u.UserId == userId))
            {
                await _db.InsertAsync(new TaskUser { TaskId = taskId, UserId = userId });
            }
        }
    }

    public async Task<bool> DeleteTaskAsync(int taskId)
    {
        var task = await GetTaskByIdAsync(taskId);
        if (task == null) return false;

        // Usuń powiązania z użytkownikami
        var taskUsers = await _db.Table<TaskUser>()
                                 .Where(tu => tu.TaskId == taskId)
                                 .ToListAsync();
        foreach (var user in taskUsers)
        {
            await _db.DeleteAsync(user);
        }

        // Usuń zadanie
        await _db.DeleteAsync(task);
        return true;
    }
}