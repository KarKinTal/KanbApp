using System.Text;
using System.Text.RegularExpressions;
using KanbApp.Models;
using KanbApp.Repositories;

namespace KanbApp.Services;

public class TableService
{
    private readonly ITableRepository _tableRepository;
    private readonly IUserRepository _userRepository;

    public TableService(ITableRepository tableRepository, IUserRepository userRepository)
    {
        _tableRepository = tableRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> CreateTableAsync(string name, int ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Table name cannot be empty.");

        var tableCode = GenerateUniqueCode();
        var newTable = new Table { Name = name, OwnerId = ownerId, TableCode = tableCode };
        await _tableRepository.AddTableAsync(newTable);
        return true;
    }

    public async Task<bool> ModifyTableAsync(Table table)
    {
        if (table == null)
            throw new ArgumentException("Invalid table data.");

        return await _tableRepository.UpdateTableAsync(table);
    }

    public async Task<Table> GetTableByIdAsync(int tableId)
    {
        return await _tableRepository.GetTableByIdAsync(tableId);
    }

    public async Task<List<Table>> GetTablesForUserAsync(int userId)
    {
        return await _tableRepository.GetTablesForUserAsync(userId);
    }

    public async Task<bool> DeleteTableAsync(int tableId, int userId)
    {
        var table = await _tableRepository.GetTableByIdAsync(tableId);

        if (table == null || table.OwnerId != userId)
            return false;

        return await _tableRepository.DeleteTableAsync(tableId);
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

    private string GenerateUniqueCode()
    {
        var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var code = new StringBuilder(4);

        for (int i = 0; i < 4; i++)
            code.Append(characters[random.Next(characters.Length)]);

        return code.ToString();
    }
}