using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using bookstore.Models;

namespace bookstore.Controllers
{
    public class AuthorsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AuthorsData/ListAuthors
        [HttpGet]
        public IEnumerable<AuthorDto> ListAuthors()
        {
            List<Author> Authors = db.Authors.ToList();
            List<AuthorDto> AuthorDtos = new List<AuthorDto>();

            Authors.ForEach(a => AuthorDtos.Add(new AuthorDto()
            {
                AuthorId = a.AuthorId,
                AuthorName = a.AuthorName,
                Biography = a.Biography,
            }));
            return AuthorDtos;
        }

        // GET: api/AuthorsData/FindAuthor/5
        [ResponseType(typeof(Author))]
        [HttpGet]
        public IHttpActionResult FindAuthor(int id)
        {
            Author Author = db.Authors.Find(id);
            AuthorDto AuthorDto = new AuthorDto()
            {
                AuthorId = Author.AuthorId,
                AuthorName = Author.AuthorName,
                Biography = Author.Biography,
            };
            if (Author == null)
            {
                return NotFound();
            }

            return Ok(AuthorDto);
        }

        // POST: api/AuthorsData/UpdateAuthor/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAuthor(int id, Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != author.AuthorId)
            {
                return BadRequest();
            }

            db.Entry(author).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AuthorsData/AddAuthor
        [ResponseType(typeof(Author))]
        [HttpPost]
        public IHttpActionResult AddAuthor(Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Authors.Add(author);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = author.AuthorId }, author);
        }

        // POST: api/AuthorsData/DeleteAuthor/5
        [ResponseType(typeof(Author))]
        [HttpPost]
        public IHttpActionResult DeleteAuthor(int id)
        {
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }

            db.Authors.Remove(author);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AuthorExists(int id)
        {
            return db.Authors.Count(e => e.AuthorId == id) > 0;
        }
    }
}