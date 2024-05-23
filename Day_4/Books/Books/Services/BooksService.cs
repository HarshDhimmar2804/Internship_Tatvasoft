using System.Collections.Generic;
using System.Linq;
using Books.Repository;
using Books.Repository.Entities;

namespace Books.Services
{
    public class BooksService
    {
        private readonly AppDbContext _cIDbContext;
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

         public void Update(Book book)
        {
            var existingBook = _cIDbContext.Books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                _cIDbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var book = GetById(id);
            if (book != null)
            {
                _cIDbContext.Books.Remove(book);
                _cIDbContext.SaveChanges(); 
            }
        }
    }
}
