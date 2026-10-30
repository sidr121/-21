using SQLite;

namespace BookLibrary.Models
{
    [Table("books")]
    public class MyData
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [NotNull, MaxLength(150)]
        public string Author { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Genre { get; set; } = string.Empty;

        public int Year { get; set; }

        public int PageCount { get; set; }

        public override string ToString()
        {
            return $"{Title} - {Author} ({Year})";
        }
    }
}
