using SQLite;

namespace KanbApp.Models
{
    public class UserWithSelection
    {
        public User User { get; set; }
        public bool IsSelected { get; set; }
    }
}
