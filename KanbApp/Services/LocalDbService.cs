using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KanbApp.Models;
using Task = System.Threading.Tasks.Task;
using AsyncAwaitBestPractices;

namespace KanbApp.Services
{
    public class LocalDbService
    {
        private const string DB_NAME = "local_db.db3";
        private readonly SQLiteAsyncConnection _connection;
        public SQLiteAsyncConnection Connection => _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            InitializeDatabaseAsync().SafeFireAndForget();
        }

        private async Task InitializeDatabaseAsync()
        {
            await _connection.CreateTableAsync<Column>();
            await _connection.CreateTableAsync<Notification>();
            await _connection.CreateTableAsync<Table>();
            await _connection.CreateTableAsync<TableUser>();
            await _connection.CreateTableAsync<Models.Task>();
            await _connection.CreateTableAsync<TaskUser>();
            await _connection.CreateTableAsync<User>();
        }

        public async Task<int> InsertItemAsync<T>(T item) where T : new()
        {
            return await _connection.InsertAsync(item);
        }

        public async Task<List<T>> GetAllItemsAsync<T>() where T : new()
        {
            return await _connection.Table<T>().ToListAsync();
        }

        public async Task<int> SafeUpdateItemAsync<T>(T item) where T : new()
        {
            try
            {
                return await _connection.UpdateAsync(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating item: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> SafeDeleteItemAsync<T>(T item) where T : new()
        {
            try
            {
                return await _connection.DeleteAsync(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting item: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> SafeInsertItemAsync<T>(T item) where T : new()
        {
            try
            {
                return await _connection.InsertAsync(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting item: {ex.Message}");
                return 0;
            }
        }
    }
}
