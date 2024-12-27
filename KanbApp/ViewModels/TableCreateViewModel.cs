using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KanbApp.Services;
using KanbApp.Pages;
using KanbApp.Models;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;
using Task = System.Threading.Tasks.Task;

namespace KanbApp.ViewModels
{
    public partial class TableCreateViewModel : BaseViewModel
    {
        private readonly TableService _tableService;
        private readonly UserService _userService;

        [ObservableProperty]
        private string tableName;

        [ObservableProperty]
        private ObservableCollection<Column> columns;

        public TableCreateViewModel(TableService tableService, UserService userService)
        {
            _tableService = tableService;
            _userService = userService;
            Columns = new ObservableCollection<Column>();
        }

        [RelayCommand]
        public async Task CreateTableAsync()
        {
            if (string.IsNullOrWhiteSpace(TableName))
            {
                await Shell.Current.DisplayAlert("Error", "Table name cannot be empty.", "OK");
                return;
            }

            if (Columns == null || !Columns.Any() || Columns.Any(c => string.IsNullOrWhiteSpace(c.Name)))
            {
                await Shell.Current.DisplayAlert("Error", "Table must have at least one column with a name.", "OK");
                return;
            }

            try
            {
                var user = await _userService.GetLoggedInUserAsync();
                if (user == null)
                {
                    await Shell.Current.DisplayAlert("Error", "You must be logged in to create a table.", "OK");
                    return;
                }

                // Tworzenie tabeli
                var tableId = await _tableService.CreateTableAsync(TableName, user.Id);
                if (tableId <= 0)
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to create table.", "OK");
                    return;
                }

                // Tworzenie kolumn
                foreach (var column in Columns)
                {
                    await _tableService.AddColumnToTableAsync(tableId, column.Name, column.ColumnNumber);
                }

                Console.WriteLine($"Navigating to TablePage with TableId: {tableId}");
                await Shell.Current.GoToAsync($"//TablePage?TableId={tableId}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to create table: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        public void AddNewColumn()
        {
            var newColumnNumber = Columns.Count + 1;
            Columns.Add(new Column { Name = $"Column {newColumnNumber}", ColumnNumber = newColumnNumber });
        }
    }
}