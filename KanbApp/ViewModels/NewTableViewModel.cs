using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KanbApp.Pages;
using KanbApp.Services;
using Mopups.Services;

namespace KanbApp.ViewModels;

public partial class NewTableViewModel : BaseViewModel
{
    private readonly TableService _tableService;
    private readonly UserService _userService;
    private readonly MainMenuViewModel _mainMenuViewModel;

    [ObservableProperty]
    private string tableCode;

    public NewTableViewModel(UserService userService, TableService tableService, MainMenuViewModel mainMenuViewModel)
    {

        _userService = userService;
        _tableService = tableService;
        _mainMenuViewModel = mainMenuViewModel;
    }

    [RelayCommand]
    public async Task JoinTableAsync()
    {
        if (string.IsNullOrWhiteSpace(TableCode))
        {
            await Shell.Current.DisplayAlert("Error", "Table code cannot be empty.", "OK");
            return;
        }

        var loggedInUser = await _userService.GetLoggedInUserAsync();
        if (loggedInUser == null)
        {
            await Shell.Current.DisplayAlert("Error", "You must be logged in to join a table.", "OK");
            return;
        }

        var table = await _tableService.GetTableByCodeAsync(TableCode.Trim());
        if (table == null)
        {
            await Shell.Current.DisplayAlert("Error", "Invalid table code.", "OK");
            return;
        }

        var isUserInTable = await _tableService.IsUserInTableAsync(table.Id, loggedInUser.Id);
        if (isUserInTable)
        {
            await Shell.Current.DisplayAlert("Info", "You are already a member of this table.", "OK");
            return;
        }

        var success = await _tableService.JoinTableByCodeAsync(TableCode.Trim(), loggedInUser.Id);

        if (success)
        {
            await Shell.Current.DisplayAlert("Success", "You have joined the table!", "OK");
            _mainMenuViewModel.LoadUserTables();
            await Shell.Current.GoToAsync(".."); // Powrót do MainMenuPage
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Invalid table code or unable to join table.", "OK");
        }
    }

    [RelayCommand]
    public async Task OpenTableCreate()
    {
        await MopupService.Instance.PopAllAsync();
        await Shell.Current.GoToAsync(nameof(TableCreatePage));
    }
}
