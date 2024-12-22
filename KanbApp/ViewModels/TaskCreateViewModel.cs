using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KanbApp.Models;
using KanbApp.Services;
using Mopups.Services;
using System.Collections.ObjectModel;
using Task = System.Threading.Tasks.Task;

namespace KanbApp.ViewModels
{
    public partial class TaskCreateViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly TaskService _taskService;
        private readonly UserService _userService;
        private readonly TableService _tableService;

        private int _currentColumnId;
        private int _currentTableId;

        [ObservableProperty]
        private string taskName;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private DateTime? selectedDate;

        [ObservableProperty]
        private ObservableCollection<User> tableUsers;

        [ObservableProperty]
        private List<int> selectedUserIds;

        public TaskCreateViewModel(TaskService taskService, UserService userService, TableService tableService)
        {
            _taskService = taskService;
            _userService = userService;
            _tableService = tableService;
            SelectedUserIds = new List<int>();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("ColumnId", out var columnId))
            {
                _currentColumnId = int.Parse(columnId.ToString());
            }

            if (query.TryGetValue("TableId", out var tableId))
            {
                _currentTableId = int.Parse(tableId.ToString());
                LoadUsersForTable(_currentTableId);
            }
        }

        private async void LoadUsersForTable(int tableId)
        {
            var users = await _tableService.GetUsersForTableAsync(tableId);
            if (users == null || users.Count==0)
            {
                System.Diagnostics.Debug.WriteLine($"No users found for table {tableId}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Loaded {users.Count} users for table {tableId}");
            }
            TableUsers = new ObservableCollection<User>(users);
        }

        [RelayCommand]
        public async Task CreateTaskAsync()
        {
            if (string.IsNullOrWhiteSpace(TaskName))
            {
                await Shell.Current.DisplayAlert("Error", "Task name cannot be empty.", "OK");
                return;
            }

            var currentUser = await _userService.GetLoggedInUserAsync();
            if (currentUser == null)
            {
                await Shell.Current.DisplayAlert("Error", "You must be logged in to create a task.", "OK");
                return;
            }

            var task = new Models.Task
            {
                Name = TaskName,
                Description = Description,
                Date = SelectedDate ?? DateTime.Now,
                OwnerId = currentUser.Id,
                ColumnId = _currentColumnId,
                TableId = _currentTableId
            };

            var taskId = await _taskService.AddTaskAsync(task);
            if (taskId > 0)
            {
                await _taskService.AssignUsersToTask(taskId, SelectedUserIds);
                await Shell.Current.DisplayAlert("Success", "Task created successfully.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to create task.", "OK");
            }
        }
    }
}