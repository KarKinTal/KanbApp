using CommunityToolkit.Mvvm.Input;
using KanbApp.Pages;
using KanbApp.ViewModels;
using Mopups.Services;
using System.Diagnostics;

namespace KanbApp.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    [RelayCommand]
    public async Task OpenTable()
    {
        await Shell.Current.GoToAsync($"//{nameof(TablePage)}");
    }

    [RelayCommand]
    public async Task OpenCreatingAccount()
    {
        await Shell.Current.GoToAsync(nameof(CreateAccountPage));
    }
}
