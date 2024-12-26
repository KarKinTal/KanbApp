using KanbApp.Services;
using KanbApp.Pages;
using KanbApp.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Task = System.Threading.Tasks.Task;

namespace KanbApp.ViewModels;

public partial class TableMenuViewModel : BaseViewModel, IQueryAttributable
{
    private readonly UserService _userService;
    private readonly TableService _tableService;

    [ObservableProperty]
    private Table currentTable;

    public TableMenuViewModel(TableService tableService, UserService userService)
    {
        _tableService = tableService;
        _userService = userService;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("TableId", out var tableIdObj) && int.TryParse(tableIdObj.ToString(), out var tableId))
        {
            CurrentTable = await _tableService.GetTableByIdAsync(tableId);
        }
    }

    [RelayCommand]
    public async Task OpenTableEdit()
    {
        if (CurrentTable == null) return;
        await Shell.Current.GoToAsync($"TableEditPage?TableId={CurrentTable.Id}");
    }

    [RelayCommand]
    public async Task CopyTableCodeAsync()
    {
        if (CurrentTable != null && !string.IsNullOrEmpty(CurrentTable.TableCode))
        {
            await Clipboard.SetTextAsync(CurrentTable.TableCode);
            await Shell.Current.DisplayAlert("Copied", "Table code has been copied to clipboard.", "OK");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Table code is not available.", "OK");
        }
    }

    [RelayCommand]
    public async Task DeleteTableAsync()
    {
        if (CurrentTable == null) return;

        var confirm = await Shell.Current.DisplayAlert("Delete Table", "Are you sure you want to delete this table? This action cannot be undone.", "Yes", "No");
        if (!confirm) return;

        var loggedInUser = await _userService.GetLoggedInUserAsync();
        if (loggedInUser == null)
        {
            await Shell.Current.GoToAsync("//LoginPage");
            return;
        }

        // Usuń tabelę i jej zależności
        var success = await _tableService.DeleteTableWithDependenciesAsync(CurrentTable.Id);

        if (success)
        {
            // Pobierz domyślną tabelę użytkownika
            var tables = await _tableService.GetTablesForUserAsync(loggedInUser.Id);
            if (tables.Any())
            {
                // Przejdź do domyślnej tabeli
                var defaultTable = tables.OrderBy(t => t.Id).First();
                await Shell.Current.GoToAsync($"//TablePage?TableId={defaultTable.Id}");
            }
            else
            {
                // Przenieś na stronę NewTablePage, jeśli brak tabel
                await Shell.Current.GoToAsync("//NewTablePage");
            }
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Failed to delete the table. Please try again.", "OK");
        }
    }
}
