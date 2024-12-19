using CommunityToolkit.Mvvm.Input;
using KanbApp.ViewModels;
using KanbApp.Pages;
using KanbApp.Services;
using KanbApp.Models;
using Mopups.Services;
using System.Collections.ObjectModel;
using Task = System.Threading.Tasks.Task;

namespace KanbApp.ViewModels;

public partial class TableViewModel : BaseViewModel
{
    private readonly UserService _userService;
    public TableViewModel(UserService userService)
    {
        _userService = userService;
    }

    [RelayCommand]
    public async Task OpenTaskCreate()
    {
        await MopupService.Instance.PushAsync(new TaskCreatePage(new TaskCreateViewModel()));
    }

    [RelayCommand]
    public async Task OpenTaskEdit()
    {
        await MopupService.Instance.PushAsync(new TaskEditPage(new TaskEditViewModel()));
    }

    [RelayCommand]
    public async Task OpenMainMenu()
    {
        await MopupService.Instance.PushAsync(new MainMenuPage(new MainMenuViewModel()));
    }

    [RelayCommand]
    public async Task OpenTableMenu()
    {
        await MopupService.Instance.PushAsync(new TableMenuPage(new TableMenuViewModel()));
    }
}
