using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class TaskCreatePage
{
    public TaskCreatePage(TaskCreateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}