using CommunityToolkit.Mvvm.Input;
using KanbApp.Pages;
using KanbApp.ViewModels;
using Mopups.Services;
using System.Diagnostics;

namespace KanbApp.ViewModels;

public partial class MainMenuViewModel : BaseViewModel
{
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
