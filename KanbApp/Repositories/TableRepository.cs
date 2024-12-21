using KanbApp.Models;
using SQLite;
using Task = System.Threading.Tasks.Task;

namespace KanbApp.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly SQLiteAsyncConnection _db;
        public TableRepository(SQLiteAsyncConnection db)
        {
            _db = db;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            _db.CreateTableAsync<Table>().Wait();
        }
        public async Task AddTableAsync(Table table)
        {
            await _db.InsertAsync(table);
        }

        public async Task<bool> UpdateTableAsync(Table table)
        {
            var result = await _db.UpdateAsync(table);
            return result > 0;
        }

        public async Task<Table> GetTableByIdAsync(int tableId)
        {
            return await _db.FindAsync<Table>(tableId);
        }

        public async Task<Table> GetTableByCodeAsync(string tableCode)
        {
            return await _db.Table<Table>().FirstOrDefaultAsync(t => t.TableCode == tableCode);
        }

        public async Task<bool> DeleteTableAsync(int tableId)
        {
            var result = await _db.DeleteAsync<Table>(tableId);
            return result > 0;
        }

        public async Task<List<Table>> GetTablesForUserAsync(int userId)
        {
            return await _db.Table<Table>().Where(t => t.OwnerId == userId).ToListAsync();
        }

        public async Task<bool> AddUserToTableAsync(int tableId, int userId)
        {
            var tableUser = new TableUser { TableId = tableId, UserId = userId };
            var result = await _db.InsertAsync(tableUser);
            return result > 0;
        }

        public async Task<bool> RemoveUserFromTableAsync(int tableId, int userId)
        {
            var tableUser = await _db.Table<TableUser>().FirstOrDefaultAsync(tu => tu.TableId == tableId && tu.UserId == userId);
            if (tableUser == null)
                return false;

            var result = await _db.DeleteAsync(tableUser);
            return result > 0;
        }

        public async Task<bool> DoesCodeExistAsync(string code)
        {
            var table = await _db.Table<Table>().FirstOrDefaultAsync(t => t.TableCode == code);
            return table != null;
        }

    }
}
