using System.Diagnostics;
using System.Text;
using KanbApp.Models;
using KanbApp.Repositories;
using SQLite;

namespace KanbApp.Services;

public class TableService
{
    private readonly ITableRepository _tableRepository;
    private readonly IUserRepository _userRepository;
    private readonly IColumnRepository _columnRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly SQLiteAsyncConnection _db;

    public TableService(ITableRepository tableRepository, IUserRepository userRepository, IColumnRepository columnRepository, ITaskRepository taskRepository, SQLiteAsyncConnection db)
    {
        _tableRepository = tableRepository;
        _userRepository = userRepository;
        _columnRepository = columnRepository;
        _taskRepository = taskRepository;
        _db = db;
    }

    public async Task<int> CreateTableAsync(string name, int ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Table name cannot be empty.");

        var tableCode = await GenerateUniqueCode();
        var newTable = new Table { Name = name, OwnerId = ownerId, TableCode = tableCode };
        await _tableRepository.AddTableAsync(newTable);

        var createdTable = await _tableRepository.GetTableByCodeAsync(tableCode);

        if (createdTable != null)
        {
            // Automatyczne przypisanie właściciela jako użytkownika tabeli
            await _tableRepository.AddUserToTableAsync(createdTable.Id, ownerId);
        }

        return createdTable?.Id ?? -1;
    }

    public async Task<bool> ModifyTableAsync(Table table)
    {
        if (table == null)
            throw new ArgumentException("Invalid table data.");

        return await _tableRepository.UpdateTableAsync(table);
    }

    public async Task<bool> ModifyColumnAsync(Column column)
    {
        if (column == null)
        {
            Debug.WriteLine("ModifyColumnAsync failed: Column is null");
            return false;
        }

        var result = await _columnRepository.UpdateColumnAsync(column);
        if (!result)
        {
            Debug.WriteLine($"ModifyColumnAsync failed: Could not update column {column.Id} in database");
        }
        return result;
    }

    public async Task<Table> GetTableByIdAsync(int tableId)
    {
        return await _tableRepository.GetTableByIdAsync(tableId);
    }

    public async Task<List<Table>> GetTablesForUserAsync(int userId)
    {
        return await _tableRepository.GetTablesForUserAsync(userId);
    }

    public async Task<Table?> GetTableByCodeAsync(string tableCode)
    {
        if (string.IsNullOrWhiteSpace(tableCode))
            return null;

        return await _tableRepository.GetTableByCodeAsync(tableCode);
    }

    public async Task<bool> LeaveTableAsync(int tableId, int userId)
    {
        var table = await _tableRepository.GetTableByIdAsync(tableId);

        if (table == null || table.OwnerId == userId)
            return false;

        return await _tableRepository.RemoveUserFromTableAsync(tableId, userId);
    }

    public async Task<bool> JoinTableByCodeAsync(string tableCode, int userId)
    {
        var table = await _tableRepository.GetTableByCodeAsync(tableCode);

        if (table == null)
            return false;

        return await _tableRepository.AddUserToTableAsync(table.Id, userId);
    }

    public async Task<bool> AddColumnToTableAsync(int tableId, string columnName, int columnNumber)
    {
        return await _columnRepository.AddColumnAsync(new Column
        {
            TableId = tableId,
            Name = columnName,
            ColumnNumber = columnNumber
        });
    }

    public async Task<bool> IsUserInTableAsync(int tableId, int userId)
    {
        var tableUser = await _db.Table<TableUser>()
                                 .FirstOrDefaultAsync(tu => tu.TableId == tableId && tu.UserId == userId);
        return tableUser != null;
    }

    public async Task<bool> DeleteTableAsync(int tableId, int userId)
    {
        var table = await _tableRepository.GetTableByIdAsync(tableId);

        if (table == null || table.OwnerId != userId)
            throw new UnauthorizedAccessException("Only the owner can delete this table.");

        return await DeleteTableWithDependenciesAsync(tableId);
    }

    public async Task<bool> DeleteTableWithDependenciesAsync(int tableId)
    {
        // Pobierz kolumny związane z tabelą
        var columns = await _columnRepository.GetColumnsByTableIdAsync(tableId);

        foreach (var column in columns)
        {
            // Pobierz zadania z każdej kolumny
            var tasks = await _taskRepository.GetTasksByColumnIdAsync(column.Id);

            foreach (var task in tasks)
            {
                // Usuń powiązania użytkowników z zadaniami
                var taskUsers = await _db.Table<TaskUser>().Where(tu => tu.TaskId == task.Id).ToListAsync();
                foreach (var taskUser in taskUsers)
                {
                    await _db.DeleteAsync(taskUser);
                }

                // Usuń zadania
                await _taskRepository.DeleteTaskAsync(task.Id);
            }

            // Usuń kolumny
            await _columnRepository.DeleteColumnAsync(column.Id);
        }

        // Usuń powiązania użytkowników z tabelą
        var tableUsers = await _db.Table<TableUser>().Where(tu => tu.TableId == tableId).ToListAsync();
        foreach (var tableUser in tableUsers)
        {
            await _db.DeleteAsync(tableUser);
        }

        // Usuń tabelę
        return await _tableRepository.DeleteTableAsync(tableId);
    }

    private async Task<string> GenerateUniqueCode()
    {
        var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = Random.Shared;
        var code = new StringBuilder(4);

        while (true)
        {
            code.Clear();
            for (int i = 0; i < 4; i++)
                code.Append(characters[random.Next(characters.Length)]);

            var generatedCode = code.ToString();
            var exists = await _tableRepository.DoesCodeExistAsync(generatedCode);
            if (!exists)
                return generatedCode;
        }
    }

    public async Task<Column> GetColumnByDetailsAsync(int tableId, string columnName, int columnNumber)
    {
        return await _columnRepository.GetColumnByDetailsAsync(tableId, columnName, columnNumber);
    }

    public async Task<List<Column>> GetColumnsForTableAsync(int tableId)
    {
        return await _columnRepository.GetColumnsByTableIdAsync(tableId);
    }

    public async Task<List<User>> GetUsersForTableAsync(int tableId)
    {
        return await _db.QueryAsync<User>(
            "SELECT u.* FROM User u " +
            "JOIN TableUser tu ON u.Id = tu.UserId " +
            "WHERE tu.TableId = ?", tableId);
    }
}