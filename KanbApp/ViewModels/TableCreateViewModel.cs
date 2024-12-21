﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KanbApp.Services;
using KanbApp.Pages;
using System.Threading.Tasks;
using System;

namespace KanbApp.ViewModels
{
    public partial class TableCreateViewModel : BaseViewModel
    {
        private readonly TableService _tableService;
        private readonly UserService _userService;

        [ObservableProperty]
        private string tableName;

        public TableCreateViewModel(TableService tableService, UserService userService)
        {
            _tableService = tableService;
            _userService = userService;
        }

        [RelayCommand]
        public async Task CreateTableAsync()
        {
            if (string.IsNullOrWhiteSpace(TableName))
            {
                await Shell.Current.DisplayAlert("Error", "Table name cannot be empty.", "OK");
                return;
            }

            try
            {
                var user = await _userService.GetLoggedInUserAsync();
                if (user == null)
                {
                    await Shell.Current.DisplayAlert("Error", "You must be logged in to create a table.", "OK");
                    return;
                }

                var tableId = await _tableService.CreateTableAsync(TableName, user.Id);
                if (tableId <= 0)
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to create table.", "OK");
                    return;
                }
                Console.WriteLine($"Navigating to TablePage with TableId: {tableId}");

                await Shell.Current.GoToAsync($"//TablePage?TableId={tableId}");


            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to create table: {ex.Message}", "OK");
            }
        }
    }
}