using SQLite;

namespace GrindscapeServer.Data
{
    public class GSCharacter
    {
        [PrimaryKey, AutoIncrement]
        public int CharacterID { get; set; }
        [Indexed]
        public int UserID { get; set; }
        [Unique, NotNull]
        public string? Name { get; set; }
    }
}
