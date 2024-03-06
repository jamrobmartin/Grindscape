using SQLite;

namespace GrindscapeServer.Data
{
    public class GSUser
    {
        [PrimaryKey, AutoIncrement]
        public int UserID { get; set; }
        [Unique, NotNull]
        public string? Username { get; set; }
        [NotNull]
        public string? Password { get; set; }
    }
}
