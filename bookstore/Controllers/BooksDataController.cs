using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using bookstore.Models;

namespace bookstore.Controllers
{
    public class BooksDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/BooksData/ListBooks
        [HttpGet]
        public IEnumerable<BookDto> ListBooks()
        {
            List<Book> Books = db.Books.ToList();
            List<BookDto> BookDtos = new List<BookDto>();
            Books.ForEach(a => BookDtos.Add(new BookDto()
            {
                BookId = a.BookId,
                Title = a.Title,
                ISBN = a.ISBN,
                Price = a.Price,
                BorrowingDate = a.BorrowingDate,
                AuthorName = a.Author.AuthorName,
            }));
            return BookDtos;
        }

        // GET: api/BooksData/FindBook/5
        [ResponseType(typeof(Book))]
        [HttpGet]
        public IHttpActionResult FindBook(int id)
        {
            Book book = db.Books.Find(id);
            BookDto BookDto = new BookDto()
            {
                BookId = book.BookId,
                Title = book.Title,
                ISBN = book.ISBN,
                Price = book.Price,
                BorrowingDate = book.BorrowingDate,
                AuthorName = book.Author.AuthorName,
            };
            if (book == null)
            {
                return NotFound();
            }

            return Ok(BookDto);
        }

        // POST: api/BooksData/UpdateBook/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateBook(int id, Book book)
        {
            Debug.WriteLine("Update Book Successful");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.BookId)
            {
                return BadRequest();
            }

            db.Entry(book).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var innerException = ex.InnerException;
                Debug.WriteLine($"DbUpdateException occurred: {innerException.Message}");
                throw;

                //if (!BookExists(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BooksData/AddBook
        [ResponseType(typeof(Book))]
        public IHttpActionResult AddBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Books.Add(book);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = book.BookId }, book);
        }

        // POST: api/BooksData/DeleteBook/5
        [ResponseType(typeof(Book))]
        [HttpPost]
        public IHttpActionResult DeleteBook(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            db.SaveChanges();

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.BookId == id) > 0;
        }
    }
}