using KanbApp.Models;
namespace KanbApp.Repositories;

public interface IColumnRepository
{
    Task<bool> AddColumnAsync(Column column);
    Task<Column> GetColumnByIdAsync(int columnId);
    Task<Column> GetColumnByDetailsAsync(int tableId, string columnName, int columnNumber);
    Task<bool> UpdateColumnAsync(Column column);
    Task<bool> DeleteColumnAsync(int columnId);
    Task<List<Column>> GetColumnsByTableIdAsync(int tableId);
}

