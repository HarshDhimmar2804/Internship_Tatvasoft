﻿using Books.Repository.Entities;
using Books.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{
    public class BooksController : Controller
    {
        private readonly BooksService _booksService;

        public BooksController(BooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet("GetBooks")]
        public ActionResult<List<Book>> Get() => _booksService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Book> Get(int id)
        {
            var book = _booksService.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return book;
        }

        [HttpPost("{id}")]
        public ActionResult<Book> Post(Book book)
        {
            _booksService.Add(book);
            return CreatedAtAction(nameof(Get), new { id = book.Id },
            book);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }
            var existingBook = _booksService.GetById(id);
            if (existingBook == null)
            {
                return NotFound();
            }
            _booksService.Update(book);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingBook = _booksService.GetById(id);
            if (existingBook == null)
            {
                return NotFound();
            }
            _booksService.Delete(id);
            return NoContent();
        }
    }
}
