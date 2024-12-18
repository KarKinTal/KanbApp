using SQLite;

namespace KanbApp.Models
{
    [Table("TableUser")]
    public class TableUser
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int TableId { get; set; } // Foreign key
        public int UserId { get; set; } // Foreign key
    }
}
