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
        //UserTables = new ObservableCollection<Table>();
        UserTables =
        [
            new Table { Id = 1, Name = "Test Table 1", OwnerId = 1 },
            new Table { Id = 2, Name = "Test Table 2", OwnerId = 2 }
        ];
        //LoadUserTables();
    }

    //private async void loadusertables()
    //{
    //    var user = await _userservice.getloggedinuserasync();
    //    if (user != null)
    //    {
    //        var tables = await _tableservice.gettablesforuserasync(user.id);
    //        usertables = new observablecollection<table>(tables);
    //    }
    //}

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
        await MopupService.Instance.PushAsync(new NewTablePage(new NewTableViewModel()));
    }

    [RelayCommand]
    public async Task OpenUserProfile()
    {
        await MopupService.Instance.PopAllAsync();
        await Shell.Current.GoToAsync(nameof(UserProfilePage));
    }
}
