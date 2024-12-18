using KanbApp.Pages;
using CommunityToolkit.Mvvm.Input;

namespace KanbApp.ViewModels;

public partial class CreateAccountViewModel : BaseViewModel
{
    [RelayCommand]
    public async Task OpenLogin()
    {
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}
