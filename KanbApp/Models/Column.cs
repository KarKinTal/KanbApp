using SQLite;

namespace KanbApp.Models
{
    [Table("Column")]
    public class Column
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public int ColumnNumber { get; set; }

        public int TableId { get; set; } // Foreign key

        public bool IsBusy { get; set; }
    }
}