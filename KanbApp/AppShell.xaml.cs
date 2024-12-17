﻿using System.Reflection;
using KanbApp.Pages;
using KanbApp.ViewModels;

namespace KanbApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CheckingLoginPage), typeof(CheckingLoginPage));
            Routing.RegisterRoute(nameof(TablePage), typeof(TablePage));
            Routing.RegisterRoute(nameof(TableCreatePage), typeof(TableCreatePage));
            Routing.RegisterRoute(nameof(TableEditPage), typeof(TableEditPage));
            Routing.RegisterRoute(nameof(UserProfilePage), typeof(UserProfilePage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }
    }
}