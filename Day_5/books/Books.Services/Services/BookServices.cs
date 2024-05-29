using Books.Models;

namespace Books.Services
{
    public class BookService
    {
        private readonly List<Book> _books;
        public BookService()
        {
            _books = new List<Book>
            {
                new Book{ id=1, Title="1984", Author="George", Genre="dystopian"}
            };
        }
        public List<Book> GetAll() 
        { 
            return _books;
        }
        public Book GetById(int id) => _books.FirstOrDefault(b => b.id == id);
        public void Add(Book book) => _books.Add(book);
        public void Update(Book book)
        {
            var index = _books.FindIndex(b => b.id == book.id);
            if (index != -1)
            {
                _books[index] = book;
            }
        }
        public void Delete(int id) // Implement this method
        {
            var book = GetById(id);
            if (book != null)
            {
                _books.Remove(book);
            }
        }
    }
}
