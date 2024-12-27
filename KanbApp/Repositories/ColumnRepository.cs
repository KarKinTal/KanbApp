using SQLite;
using KanbApp.Models;
using System.Diagnostics;

namespace KanbApp.Repositories;

public class ColumnRepository : IColumnRepository
{
    private readonly SQLiteAsyncConnection _db;

    public ColumnRepository(SQLiteAsyncConnection db)
    {
        _db = db;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        _db.CreateTableAsync<Column>().Wait();
    }

    public async Task<bool> AddColumnAsync(Column column)
    {
        var result = await _db.InsertAsync(column);
        return result > 0;
    }

    public async Task<Column> GetColumnByDetailsAsync(int tableId, string columnName, int columnNumber)
    {
        return await _db.Table<Column>()
            .FirstOrDefaultAsync(c => c.TableId == tableId && c.Name == columnName && c.ColumnNumber == columnNumber);
    }

    public async Task<Column> GetColumnByIdAsync(int columnId)
    {
        return await _db.FindAsync<Column>(columnId);
    }

    public async Task<bool> UpdateColumnAsync(Column column)
    {
        try
        {
            var result = await _db.UpdateAsync(column);
            Debug.WriteLine(result > 0
                ? $"Successfully updated column {column.Id}"
                : $"Failed to update column {column.Id}");
            return result > 0;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating column {column.Id}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteColumnAsync(int columnId)
    {
        var result = await _db.DeleteAsync<Column>(columnId);
        return result > 0;
    }

    public async Task<List<Column>> GetColumnsByTableIdAsync(int tableId)
    {
        return await _db.Table<Column>().Where(c => c.TableId == tableId).OrderBy(c => c.ColumnNumber).ToListAsync();
    }
}