using KanbApp.Models;
using KanbApp.Repositories;

namespace KanbApp.Services;

public class ColumnService
{
    private readonly IColumnRepository _columnRepository;
    private readonly ITaskRepository _taskRepository;

    public ColumnService(IColumnRepository columnRepository, ITaskRepository taskRepository)
    {
        _columnRepository = columnRepository;
        _taskRepository = taskRepository;
    }

    public async Task<bool> AddColumnAsync(int tableId, string name, int columnNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Column name cannot be empty.");

        var newColumn = new Column { TableId = tableId, Name = name, ColumnNumber = columnNumber };
        return await _columnRepository.AddColumnAsync(newColumn);
    }

    public async Task<bool> RemoveColumnAsync(int columnId)
    {
        var tasksInColumn = await _taskRepository.GetTasksByColumnIdAsync(columnId);

        if (tasksInColumn.Any())
            return false;

        return await _columnRepository.DeleteColumnAsync(columnId);
    }

    public async Task<bool> ChangeColumnPositionAsync(int columnId, int newColumnNumber)
    {
        var column = await _columnRepository.GetColumnByIdAsync(columnId);

        if (column == null)
            return false;

        column.ColumnNumber = newColumnNumber;
        return await _columnRepository.UpdateColumnAsync(column);
    }

    public async Task<bool> ColumnContainsTasksAsync(int columnId)
    {
        var tasksInColumn = await _taskRepository.GetTasksByColumnIdAsync(columnId);
        return tasksInColumn.Any();
    }
}