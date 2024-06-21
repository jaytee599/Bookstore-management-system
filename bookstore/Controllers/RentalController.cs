using bookstore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace bookstore.Controllers
{
    public class RentalController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static RentalController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44383/api/"); 
        }

        // GET: Rental/List
        public ActionResult List()
        {
            string url = "rentalsdata/listrentals";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<RentalDto> rentals = response.Content.ReadAsAsync<IEnumerable<RentalDto>>().Result;

            return View(rentals);
        }

        // GET: Rental/Details/5
        public ActionResult Details(int id)
        {
            string url = "rentalsdata/findrental/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RentalDto selectedRental = response.Content.ReadAsAsync<RentalDto>().Result;

            return View(selectedRental);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Rental/New
        public ActionResult New()
        {
            string url = "booksdata/listbooks";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<BookDto> BookOptions = response.Content.ReadAsAsync<IEnumerable<BookDto>>().Result;

            return View(BookOptions);
        }

        // POST: Rental/Create
        [HttpPost]
        public ActionResult Create(Rental rental)
        {
            string url = "rentalsdata/addrental";
            string jsonpayload = jss.Serialize(rental);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Rental/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "rentalsdata/findrental/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RentalDto selectedRental = response.Content.ReadAsAsync<RentalDto>().Result;

            return View(selectedRental);
        }

        // POST: Rental/Update/5
        [HttpPost]
        public ActionResult Update(int id, Rental rental)
        {
            string url = "rentalsdata/updaterental/" + id;
            string jsonpayload = jss.Serialize(rental);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"DELETE request failed with status code: {response.StatusCode}");
                Debug.WriteLine($"Error message: {errorMessage}");
                return RedirectToAction("Error");
            }
        }

        // GET: Rental/Delete/5
        public ActionResult ConfirmDelete(int id)
        {
            string url = "rentalsdata/findrental/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RentalDto selectedRental = response.Content.ReadAsAsync<RentalDto>().Result;

            return View(selectedRental);
        }

        // POST: Rental/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "rentalsdata/deleterental/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
