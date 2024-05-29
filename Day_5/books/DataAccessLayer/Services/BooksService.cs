using DataAccessLayer.Repository;
using DataAccessLayer.Repository.Entities;

namespace DataAccessLayer.BooksService
{
  
    public class BooksService
    {
        private readonly AppDbContext _cIDbContext;
        private int _nextId = 2;
        public BooksService(AppDbContext cIDbContext)
        {
            _cIDbContext = cIDbContext;
        }

        public List<Book> GetAll()
        {
            return _cIDbContext.Books.ToList();
        }

        public Book GetById(int id) => _cIDbContext.Books.FirstOrDefault(b => b.Id == id);

        public void Add(Book book)
        {
            _cIDbContext.Books.Add(book);
            _cIDbContext.SaveChanges();
        }
        public void update(Book book)
        {
            var existingBook = _cIDbContext.Books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Genre = book.Genre;
                existingBook.Price = book.Price;
                _cIDbContext.SaveChanges();
            }
        }
        public void Delete(int id)
        {
            var book = GetById(id);
            if (book != null)
            {
                _cIDbContext.Remove(book);
                _cIDbContext.SaveChanges();
            }
        }
        public List<Book> PriceLessThan(int maxPrice)
        {
            return _cIDbContext.Books.Where(b => b.Price < maxPrice).ToList();
        }
    }
}

