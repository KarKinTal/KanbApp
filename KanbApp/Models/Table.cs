using SQLite;

namespace KanbApp.Models
{
    [Table("Table")]
    public class Table
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public int OwnerId { get; set; } // Foreign key

        public string TableCode { get; set; }
    }
}
