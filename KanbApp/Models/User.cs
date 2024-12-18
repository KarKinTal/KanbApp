using SQLite;

namespace KanbApp.Models
{
    [Table("User")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; } // Should be hashed in practice
    }
}