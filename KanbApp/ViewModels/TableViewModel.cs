using CommunityToolkit.Mvvm.Input;
using KanbApp.ViewModels;
using KanbApp.Pages;
using KanbApp.Services;
using KanbApp.Models;
using Mopups.Services;
using System.Collections.ObjectModel;
using Task = System.Threading.Tasks.Task;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KanbApp.ViewModels;

public partial class TableViewModel : BaseViewModel, IQueryAttributable
{
    private readonly UserService _userService;
    private readonly TableService _tableService;
    private MainMenuViewModel _mainMenuViewModel;

    [ObservableProperty]
    private Table currentTable;
    [ObservableProperty]
    private List<Column> columns;

    public TableViewModel(UserService userService, TableService tableService)
    {
        _userService = userService;
        _tableService = tableService;
        Columns = new List<Column>();
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("TableId", out var tableIdObj) && int.TryParse(tableIdObj.ToString(), out var tableId))
        {
            // Jeśli TableId jest podane, załaduj tę tabelę
            await LoadTableByIdAsync(tableId);
        }
        else
        {
            // Jeśli nie podano TableId, załaduj domyślną tabelę użytkownika
            await LoadDefaultTableForUserAsync();
        }
    }

    public async Task LoadTableByIdAsync(int tableId)
    {
        CurrentTable = await _tableService.GetTableByIdAsync(tableId);
        if (CurrentTable != null)
        {
            Columns = await _tableService.GetColumnsForTableAsync(tableId);
        }
    }

    public async Task LoadDefaultTableForUserAsync()
    {
        var user = await _userService.GetLoggedInUserAsync();
        if (user == null)
            return;

        var tables = await _tableService.GetTablesForUserAsync(user.Id);
        var defaultTable = tables.OrderBy(t => t.Id).FirstOrDefault();

        if (defaultTable != null)
        {
            await LoadTableByIdAsync(defaultTable.Id);
        }
    }

    [RelayCommand]
    public async Task OpenTaskCreate()
    {
        await Shell.Current.GoToAsync(nameof(TaskCreatePage));
    }

    [RelayCommand]
    public async Task OpenTaskEdit()
    {
        await Shell.Current.GoToAsync(nameof(TaskEditPage));
    }

    [RelayCommand]
    public async Task OpenMainMenu()
    {
        if (_mainMenuViewModel == null)
        {
            _mainMenuViewModel = new MainMenuViewModel(_tableService, _userService);
        }
        await Shell.Current.GoToAsync(nameof(MainMenuPage));
    }

    [RelayCommand]
    public async Task OpenTableMenu()
    {
        await Shell.Current.GoToAsync(nameof(TableMenuPage));
    }
}
