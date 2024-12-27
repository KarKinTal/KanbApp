using KanbApp.Models;
using SQLite;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace KanbApp.Services
{
    public class NotificationService
    {
        private readonly SQLiteAsyncConnection _db;

        public NotificationService(SQLiteAsyncConnection db)
        {
            _db = db;
            _db.CreateTableAsync<Notification>().Wait();
        }

        public async Task SaveNotificationSettingsAsync(int userId, bool isOn, int howMuchBefore)
        {
            var existingNotification = await _db.Table<Notification>().FirstOrDefaultAsync(n => n.UserId == userId);

            if (existingNotification == null)
            {
                // Dodaj nowe ustawienie, jeśli jeszcze nie istnieje
                var newNotification = new Notification
                {
                    UserId = userId,
                    IsOn = isOn,
                    HowMuchBefore = howMuchBefore
                };
                await _db.InsertAsync(newNotification);
            }
            else
            {
                // Zaktualizuj istniejące ustawienie
                existingNotification.IsOn = isOn;
                existingNotification.HowMuchBefore = howMuchBefore;
                await _db.UpdateAsync(existingNotification);
            }
        }

        public async Task<Notification> GetNotificationSettingsAsync(int userId)
        {
            var notification = await _db.Table<Notification>().FirstOrDefaultAsync(n => n.UserId == userId);

            if (notification == null)
            {
                // Domyślne ustawienia, jeśli użytkownik jeszcze ich nie skonfigurował
                return new Notification
                {
                    UserId = userId,
                    IsOn = false,
                    HowMuchBefore = 1 // Domyślna liczba dni
                };
            }

            return notification;
        }
    }
}