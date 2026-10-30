using SQLite;
using BookLibrary.Models;

namespace BookLibrary.Services
{
    public class MyDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public MyDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<MyData>().Wait();
        }

        public Task<List<MyData>> GetAllDataAsync()
        {
            return _database.Table<MyData>().ToListAsync();
        }

        public async Task<List<MyData>> SearchBooksAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return await GetAllDataAsync();

            var books = await _database.Table<MyData>().ToListAsync();
            return books.Where(b =>
                b.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                (b.Genre != null && b.Genre.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        public Task<int> AddBookAsync(MyData book)
        {
            return _database.InsertAsync(book);
        }

        public Task<int> DeleteBookAsync(MyData book)
        {
            return _database.DeleteAsync(book);
        }
    }
}
