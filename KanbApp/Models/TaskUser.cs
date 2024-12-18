using SQLite;

namespace KanbApp.Models
{
    [Table("TaskUser")]
    public class TaskUser
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int TaskId { get; set; } // Foreign key
        public int UserId { get; set; } // Foreign key
    }
}
