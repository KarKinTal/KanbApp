﻿using CommunityToolkit.Mvvm.Input;
using KanbApp.Services;
using KanbApp.Pages;
using Mopups.Services;
using Mopups.PreBaked.PopupPages.Login;

namespace KanbApp.ViewModels;

public partial class UserProfileViewModel : BaseViewModel
{
    private readonly UserService _userService;

    public UserProfileViewModel(UserService userService)
    {
        _userService = userService;
    }

    [RelayCommand]
    public async Task OpenChangingUserData()
    {
        await MopupService.Instance.PushAsync(new ChangeUserDataPage(new ChangeUserDataViewModel()));
    }


    [RelayCommand]
    public async Task Logout()
    {
        try
        {
            await _userService.LogoutAsync();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
