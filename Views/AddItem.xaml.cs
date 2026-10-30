using BookLibrary.Models;
using BookLibrary.Services;

namespace BookLibrary.Views
{
    public partial class AddItem : ContentPage
    {
        private readonly MyDatabase _database;

        public AddItem()
        {
            InitializeComponent();
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "BookLibrary.db3");
            _database = new MyDatabase(dbPath);
        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleEntry.Text))
            {
                await DisplayAlert("Ошибка", "Введите название книги", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(AuthorEntry.Text))
            {
                await DisplayAlert("Ошибка", "Введите автора книги", "OK");
                return;
            }

            int year = 0;
            if (!string.IsNullOrWhiteSpace(YearEntry.Text))
            {
                if (!int.TryParse(YearEntry.Text, out year) || year < 0 || year > DateTime.Now.Year + 1)
                {
                    await DisplayAlert("Ошибка", "Введите корректный год издания", "OK");
                    return;
                }
            }

            int pageCount = 0;
            if (!string.IsNullOrWhiteSpace(PageCountEntry.Text))
            {
                if (!int.TryParse(PageCountEntry.Text, out pageCount) || pageCount <= 0)
                {
                    await DisplayAlert("Ошибка", "Введите корректное количество страниц", "OK");
                    return;
                }
            }

            var book = new MyData
            {
                Title = TitleEntry.Text.Trim(),
                Author = AuthorEntry.Text.Trim(),
                Genre = GenreEntry.Text?.Trim(),
                Year = year,
                PageCount = pageCount
            };

            await _database.AddBookAsync(book);

            TitleEntry.Text = string.Empty;
            AuthorEntry.Text = string.Empty;
            GenreEntry.Text = string.Empty;
            YearEntry.Text = string.Empty;
            PageCountEntry.Text = string.Empty;

            await DisplayAlert("Успех", "Книга успешно добавлена!", "OK");
        }
    }
}
