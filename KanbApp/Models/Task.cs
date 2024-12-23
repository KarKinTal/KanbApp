using SQLite;

namespace KanbApp.Models
{
    [Table("Task")]
    public class Task
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ColumnId { get; set; } // Foreign key
        public int TableId { get; set; } // Foreign key
        public int OwnerId { get; set; } // Foreign key

        public string Name { get; set; }

        public string Description { get; set; }
        public string UsersNames { get; set; }
        public DateTime Date { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsCollapsed => !IsExpanded;
    }
}
