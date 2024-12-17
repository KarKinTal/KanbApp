using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class TablePage
{
    public TablePage(TableViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}