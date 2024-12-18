using KanbApp.Models;

namespace KanbApp.Repositories;

public interface ITableRepository
{
    Task<List<Table>> GetTablesForUserAsync(int userId);
}
