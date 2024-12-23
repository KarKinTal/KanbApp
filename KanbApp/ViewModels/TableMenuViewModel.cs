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
        await Shell.Current.GoToAsync(nameof(TableEditPage));
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
}
