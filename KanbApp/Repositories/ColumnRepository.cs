﻿using SQLite;
using KanbApp.Models;

namespace KanbApp.Repositories;

public class ColumnRepository : IColumnRepository
{
    private readonly SQLiteAsyncConnection _db;

    public ColumnRepository(SQLiteAsyncConnection db)
    {
        _db = db;
    }

    public async Task<bool> AddColumnAsync(Column column)
    {
        var result = await _db.InsertAsync(column);
        return result > 0;
    }

    public async Task<Column> GetColumnByIdAsync(int columnId)
    {
        return await _db.FindAsync<Column>(columnId);
    }

    public async Task<bool> UpdateColumnAsync(Column column)
    {
        var result = await _db.UpdateAsync(column);
        return result > 0;
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