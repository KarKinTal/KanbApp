using SQLite;

namespace KanbApp.Models
{
    [Table("Notification")]
    public class Notification
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; } // Foreign key

        public bool IsOn { get; set; }

        public int HowMuchBefore { get; set; } // Time in minutes before an event
    }
}
