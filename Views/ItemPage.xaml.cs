using BookLibrary.Models;
using BookLibrary.Services;

namespace BookLibrary.Views
{
    public partial class ItemPage : ContentPage
    {
        private readonly MyDatabase _database;
        private MyData _selectedBook;

        public ItemPage()
        {
            InitializeComponent();
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "BookLibrary.db3");
            _database = new MyDatabase(dbPath);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadBooksAsync();
        }

        private async Task LoadBooksAsync()
        {
            var books = await _database.GetAllDataAsync();
            BooksListView.ItemsSource = books;
            InfoLabel.Text = $"Всего книг: {books.Count}";
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var books = await _database.SearchBooksAsync(SearchBar.Text);
            BooksListView.ItemsSource = books;
            InfoLabel.Text = $"Найдено книг: {books.Count}";
        }

        private void BooksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedBook = e.CurrentSelection.FirstOrDefault() as MyData;
            if (_selectedBook != null)
            {
                InfoLabel.Text = $"Выбрана: {_selectedBook.Title}";
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (_selectedBook == null)
            {
                await DisplayAlert("Ошибка", "Сначала выберите книгу из списка", "OK");
                return;
            }

            bool confirm = await DisplayAlert("Подтверждение", $"Удалить книгу \"{_selectedBook.Title}\"?", "Да", "Нет");
            if (confirm)
            {
                await _database.DeleteBookAsync(_selectedBook);
                _selectedBook = null;
                await LoadBooksAsync();
                await DisplayAlert("Успех", "Книга удалена", "OK");
            }
        }

        private async void UpdateButton_Clicked(object sender, EventArgs e)
        {
            await LoadBooksAsync();
            await DisplayAlert("Успех", "Список обновлён", "OK");
        }
    }
}
