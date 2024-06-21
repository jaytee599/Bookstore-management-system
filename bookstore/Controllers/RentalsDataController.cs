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
using bookstore.Migrations;
using bookstore.Models;

namespace bookstore.Controllers
{
    public class RentalsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/RentalsData/ListRentals
        [HttpGet]
        public IEnumerable<RentalDto> ListRentals()
        {
            List<Rental> Rentals = db.Rentals.ToList();
            List<RentalDto> RentalDtos = new List<RentalDto>();
            Rentals.ForEach(a => RentalDtos.Add(new RentalDto()
            {
                RentalId = a.RentalId,
                ISBN = a.ISBN,
                RentalDate = a.RentalDate,
                DueDate = a.DueDate,
                RentalAvailable = a.RentalAvailable,
                Title = a.Book.Title,
            }));
            return RentalDtos;
        }

        // GET: api/RentalsData/FindRental/5
        [ResponseType(typeof(Rental))]
        [HttpGet]
        public IHttpActionResult FindRental(int id)
        {
            Rental Rental = db.Rentals.Find(id);
            RentalDto RentalDto = new RentalDto()
            {
                RentalId = Rental.RentalId,
                ISBN = Rental.ISBN,
                RentalDate = Rental.RentalDate,
                DueDate = Rental.DueDate,
                RentalAvailable = Rental.RentalAvailable,
                Title = Rental.Book.Title,
            };
            if (Rental == null)
            {
                return NotFound();
            }

            return Ok(RentalDto);
        }

        // POST: api/RentalsData/UpdateRental/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRental(int id, Rental rental)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rental.RentalId)
            {
                return BadRequest();
            }

            db.Entry(rental).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalExists(id))
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

        // POST: api/RentalsData/AddRental
        [ResponseType(typeof(Rental))]
        [HttpPost]
        public IHttpActionResult AddRental(Rental rental)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rentals.Add(rental);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = rental.RentalId }, rental);
        }

        // POST: api/RentalsData/DeleteRental/5
        [ResponseType(typeof(Rental))]
        [HttpPost]
        public IHttpActionResult DeleteRental(int id)
        {
            Rental rental = db.Rentals.Find(id);
            if (rental == null)
            {
                return NotFound();
            }

            db.Rentals.Remove(rental);
            db.SaveChanges();

            return Ok(rental);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RentalExists(int id)
        {
            return db.Rentals.Count(e => e.RentalId == id) > 0;
        }
    }
}