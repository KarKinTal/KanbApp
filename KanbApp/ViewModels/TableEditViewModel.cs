using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KanbApp.Models;
using KanbApp.Services;
using System.Collections.ObjectModel;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace KanbApp.ViewModels
{
    public partial class TableEditViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly TableService _tableService;
        private readonly ColumnService _columnService;

        public TableEditViewModel(TableService tableService, ColumnService columnService)
        {
            _tableService = tableService;
            _columnService = columnService;
            Columns = new ObservableCollection<Column>();
        }

        [ObservableProperty]
        private Table currentTable;

        [ObservableProperty]
        private ObservableCollection<Column> columns;

        [ObservableProperty]
        private string newColumnName;

        public async Task InitializeAsync(int tableId)
        {
            CurrentTable = await _tableService.GetTableByIdAsync(tableId);
            var columns = await _tableService.GetColumnsForTableAsync(tableId);
            Columns = new ObservableCollection<Column>(columns.OrderBy(c => c.ColumnNumber));
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("TableId", out var tableIdObj) && int.TryParse(tableIdObj.ToString(), out var tableId))
            {
                await LoadTableAsync(tableId);
            }
        }

        private async Task LoadTableAsync(int tableId)
        {
            CurrentTable = await _tableService.GetTableByIdAsync(tableId);

            if (CurrentTable != null)
            {
                var columns = await _tableService.GetColumnsForTableAsync(CurrentTable.Id);
                Columns = new ObservableCollection<Column>(columns);
            }
        }

        private async Task RefreshColumnsAsync()
        {
            var updatedColumns = await _tableService.GetColumnsForTableAsync(CurrentTable.Id);
            Columns = new ObservableCollection<Column>(updatedColumns.OrderBy(c => c.ColumnNumber));
        }

        [RelayCommand]
        public async Task AddNewColumnAsync()
        {
            if (CurrentTable == null)
            {
                await Shell.Current.DisplayAlert("Error", "Table data is invalid.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewColumnName))
            {
                await Shell.Current.DisplayAlert("Error", "Column name cannot be empty.", "OK");
                return;
            }

            var newColumnNumber = Columns.Any() ? Columns.Max(c => c.ColumnNumber) + 1 : 1;

            var success = await _tableService.AddColumnToTableAsync(CurrentTable.Id, NewColumnName, newColumnNumber);

            if (success)
            {
                await RefreshColumnsAsync(); // Odśwież listę kolumn
                NewColumnName = string.Empty;

                await Shell.Current.DisplayAlert("Success", "Column added successfully.", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to add column.", "OK");
            }
        }

        [RelayCommand]
        public async Task RemoveColumnAsync(Column column)
        {
            if (column == null) return;

            // Sprawdź, czy kolumna jest jedyną w tabeli
            if (Columns.Count == 1)
            {
                await Shell.Current.DisplayAlert("Error", "You cannot delete the only column in the table.", "OK");
                return;
            }

            // Sprawdź, czy kolumna zawiera zadania
            var containsTasks = await _columnService.ColumnContainsTasksAsync(column.Id);
            if (containsTasks)
            {
                await Shell.Current.DisplayAlert("Error", "Cannot delete a column containing tasks.", "OK");
                return;
            }

            // Usuń kolumnę z bazy danych
            var success = await _columnService.RemoveColumnAsync(column.Id);
            if (success)
            {
                // Usuń kolumnę z lokalnej kolekcji
                Columns.Remove(column);

                // Zaktualizuj numerację pozostałych kolumn
                var updatedColumns = Columns
                    .Where(c => c.ColumnNumber > column.ColumnNumber)
                    .OrderBy(c => c.ColumnNumber)
                    .ToList();

                foreach (var updatedColumn in updatedColumns)
                {
                    updatedColumn.ColumnNumber--;
                    await _tableService.ModifyColumnAsync(updatedColumn); // Aktualizuj w bazie danych
                }

                // Odśwież widok kolumn
                Columns = new ObservableCollection<Column>(Columns.OrderBy(c => c.ColumnNumber));
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to remove column.", "OK");
            }
        }

        [RelayCommand]
        public async Task MoveColumnUpAsync(Column column)
        {
            if (column == null || column.ColumnNumber <= 1) return;

            var targetColumn = Columns.FirstOrDefault(c => c.ColumnNumber == column.ColumnNumber - 1);
            if (targetColumn != null)
            {
                // Zamiana numerów kolumn
                int originalColumnNumber = column.ColumnNumber;
                column.ColumnNumber = targetColumn.ColumnNumber;
                targetColumn.ColumnNumber = originalColumnNumber;

                // Aktualizacja w bazie danych
                await _tableService.ModifyColumnAsync(column);
                await _tableService.ModifyColumnAsync(targetColumn);

                // Odśwież listę kolumn
                Columns = new ObservableCollection<Column>(Columns.OrderBy(c => c.ColumnNumber));
            }
        }

        [RelayCommand]
        public async Task MoveColumnDownAsync(Column column)
        {
            if (column == null || column.ColumnNumber >= Columns.Count) return;

            var targetColumn = Columns.FirstOrDefault(c => c.ColumnNumber == column.ColumnNumber + 1);
            if (targetColumn != null)
            {
                // Zamiana numerów kolumn
                int originalColumnNumber = column.ColumnNumber;
                column.ColumnNumber = targetColumn.ColumnNumber;
                targetColumn.ColumnNumber = originalColumnNumber;

                // Aktualizacja w bazie danych
                await _tableService.ModifyColumnAsync(column);
                await _tableService.ModifyColumnAsync(targetColumn);

                // Odśwież listę kolumn
                Columns = new ObservableCollection<Column>(Columns.OrderBy(c => c.ColumnNumber));
            }
        }

        [RelayCommand]
        public async Task UpdateColumnNameAsync(Column column)
        {
            if (column == null || string.IsNullOrWhiteSpace(column.Name))
            {
                await Shell.Current.DisplayAlert("Error", "Column name cannot be empty.", "OK");
                return;
            }

            var success = await _tableService.ModifyColumnAsync(column);

            if (!success)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to update column name.", "OK");
            }
        }

        [RelayCommand]
        public async Task SaveChangesAsync()
        {
            if (CurrentTable == null)
            {
                await Shell.Current.DisplayAlert("Error", "Table data is invalid.", "OK");
                return;
            }

            var success = await _tableService.ModifyTableAsync(CurrentTable);

            if (success)
            {
                await Shell.Current.DisplayAlert("Success", "Table updated successfully.", "OK");
                await Shell.Current.GoToAsync($"///TablePage?TableId={CurrentTable.Id}"); // Trzy slashe, by wymusić pełną nawigację
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to update the table.", "OK");
            }
        }
    }
}