using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KanbApp.Models;
using KanbApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace KanbApp.ViewModels;

public class UserWithSelection
{
    public User User { get; set; }
    public bool IsSelected { get; set; }
}

public partial class TaskEditViewModel : BaseViewModel, IQueryAttributable
{
    private readonly TaskService _taskService;
    private readonly TableService _tableService;
    private readonly TableViewModel _tableViewModel;

    [ObservableProperty]
    private Models.Task currentTask;

    [ObservableProperty]
    private ObservableCollection<UserWithSelection> usersWithSelection;

    public TaskEditViewModel(TaskService taskService, TableService tableService, TableViewModel tableViewModel)
    {
        _taskService = taskService;
        _tableService = tableService;
        _tableViewModel = tableViewModel;
        UsersWithSelection = new ObservableCollection<UserWithSelection>();
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("TaskId", out var taskIdObj) && int.TryParse(taskIdObj.ToString(), out var taskId))
        {
            await LoadTaskAsync(taskId);
        }
    }

    private async Task LoadTaskAsync(int taskId)
    {
        CurrentTask = await _taskService.GetTaskByIdAsync(taskId);
        if (CurrentTask != null)
        {
            var allUsers = await _tableService.GetUsersForTableAsync(CurrentTask.TableId);
            var taskUsers = await _taskService.GetUsersForTaskAsync(taskId);

            UsersWithSelection = new ObservableCollection<UserWithSelection>(
                allUsers.Select(user => new UserWithSelection
                {
                    User = user,
                    IsSelected = taskUsers.Any(tu => tu.Id == user.Id)
                }));
        }
    }

    [RelayCommand]
    public async Task SaveTaskAsync()
    {
        if (CurrentTask == null) return;

        // Aktualizacja zadania
        var success = await _taskService.UpdateTaskAsync(CurrentTask);

        // Aktualizacja przypisanych użytkowników
        var assignedUserIds = UsersWithSelection
            .Where(uws => uws.IsSelected)
            .Select(uws => uws.User.Id)
            .ToList();
        await _taskService.UpdateTaskUsersAsync(CurrentTask.Id, assignedUserIds);

        if (success)
        {
            await _tableViewModel.RefreshTasksAsync();
            await Shell.Current.DisplayAlert("Success", "Task updated successfully.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Failed to update the task.", "OK");
        }
    }

    [RelayCommand]
    public async Task DeleteTaskAsync()
    {
        if (CurrentTask == null) return;

        var confirm = await Shell.Current.DisplayAlert("Confirm", "Are you sure you want to delete this task?", "Yes", "No");
        if (!confirm) return;

        var success = await _taskService.DeleteTaskAsync(CurrentTask.Id);
        if (success)
        {
            await Shell.Current.DisplayAlert("Success", "Task deleted successfully.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Failed to delete the task.", "OK");
        }
    }
}