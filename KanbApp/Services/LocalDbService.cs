using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KanbApp.Models;

namespace KanbApp.Services
{
    public class LocalDbService
    {
        private const string DB_NAME = "local_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory,DB_NAME));
            _connection.CreateTableAsync<Column>();
            _connection.CreateTableAsync<Notification>();
            _connection.CreateTableAsync<Table>();
            _connection.CreateTableAsync<TableUser>();
            _connection.CreateTableAsync<Models.Task>();
            _connection.CreateTableAsync<TaskUser>(); 
            _connection.CreateTableAsync<User>();
        }
    }
}
