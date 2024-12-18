using KanbApp.Models;
using SQLite;

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

        public async Task<List<Table>> GetTablesForUserAsync(int userId)
        {
            return await _db.Table<Table>().Where(t => t.OwnerId == userId).ToListAsync();
        }

    }
}
