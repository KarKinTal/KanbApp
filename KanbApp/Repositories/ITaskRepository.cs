using KanbApp.Models;
namespace KanbApp.Repositories;

public interface ITaskRepository
{
    Task<List<Models.Task>> GetTasksByColumnIdAsync(int columnId);

}

