using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class MainMenuPage
{
    public MainMenuPage(MainMenuViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}