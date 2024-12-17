using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class TableCreatePage
{
    public TableCreatePage(TableCreateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}