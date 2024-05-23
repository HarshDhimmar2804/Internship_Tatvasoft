using System.Collections.Generic;
using System.Linq;
using Books.Models;

namespace Books.Services
{
    public class BooksService
    {
        private readonly List<Book> _books;
        private int _nextId = 2; // Start from 2 because we have one book with Id = 1

        public BooksService()
        {
            _books = new List<Book>
            {
                new Book { Id = 1, Title = "1984", Author = "George Orwell", Genre = "Dystopian" },
            };
        }

        public List<Book> GetAll()
        {
            return _books;
        }

        public Book GetById(int id) => _books.FirstOrDefault(b => b.Id == id);

        public void Add(Book book)
        {
            book.Id = _nextId++;
            _books.Add(book);
        }

        public void Update(Book book)
        {
            var index = _books.FindIndex(b => b.Id == book.Id);
            if (index != -1)
            {
                _books[index] = book;
            }
        }

        public void Delete(int id)
        {
            var book = GetById(id);
            if (book != null)
            {
                _books.Remove(book);
            }
        }
    }
}
