using System.Reflection;
using KanbApp.Pages;
using KanbApp.ViewModels;

namespace KanbApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
            Routing.RegisterRoute(nameof(ChangeUserDataPage), typeof(ChangeUserDataPage));
            Routing.RegisterRoute(nameof(CheckingLoginPage), typeof(CheckingLoginPage));
            Routing.RegisterRoute(nameof(CreateAccountPage), typeof(CreateAccountPage));
            Routing.RegisterRoute(nameof(MainMenuPage), typeof(MainMenuPage));
            Routing.RegisterRoute(nameof(NewTablePage), typeof(NewTablePage));
            Routing.RegisterRoute(nameof(TablePage), typeof(TablePage));
            Routing.RegisterRoute(nameof(TableCreatePage), typeof(TableCreatePage));
            Routing.RegisterRoute(nameof(TableEditPage), typeof(TableEditPage));
            Routing.RegisterRoute(nameof(TableMenuPage), typeof(TableMenuPage));
            Routing.RegisterRoute(nameof(TaskCreatePage), typeof(TaskCreatePage));
            Routing.RegisterRoute(nameof(TaskEditPage), typeof(TaskEditPage));
            Routing.RegisterRoute(nameof(UserProfilePage), typeof(UserProfilePage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }
    }
}