using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KanbApp.Models;
using KanbApp.Pages;
using KanbApp.Services;
using KanbApp.ViewModels;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Task = System.Threading.Tasks.Task;

namespace KanbApp.ViewModels;

public partial class MainMenuViewModel : BaseViewModel
{
    private readonly TableService _tableService;
    private readonly UserService _userService;

    [ObservableProperty]
    private ObservableCollection<Table> userTables;

    public MainMenuViewModel(TableService tableService, UserService userService)
    {
        _tableService = tableService ?? throw new ArgumentNullException(nameof(tableService));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        UserTables = new ObservableCollection<Table>();
        LoadUserTables();
    }

    public async void LoadUserTables()
    {
        var user = await _userService.GetLoggedInUserAsync();
        if (user != null)
        {
            var tables = await _tableService.GetTablesForUserAsync(user.Id);
            UserTables = new ObservableCollection<Table>(tables);
        }
    }

    public void RefreshTables()
    {
        LoadUserTables();
    }

    [RelayCommand]
    public async Task OpenTable(int tableId)
    {
        await MopupService.Instance.PopAllAsync();
        await Shell.Current.GoToAsync($"//TablePage?TableId={tableId}");
    }

    [RelayCommand]
    public async Task OpenNewTable()
    {
        await MopupService.Instance.PopAllAsync();
        await Shell.Current.GoToAsync(nameof(NewTablePage));
    }

    [RelayCommand]
    public async Task OpenUserProfile()
    {
        await MopupService.Instance.PopAllAsync();
        await Shell.Current.GoToAsync(nameof(UserProfilePage));
    }
}
