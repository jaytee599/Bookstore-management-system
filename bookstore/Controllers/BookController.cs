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
    public class BookController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BookController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44383/api/");
        }
        // GET: Book/List
        public ActionResult List()
        {

            string url = "booksdata/listbooks";

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<BookDto> books = response.Content.ReadAsAsync<IEnumerable<BookDto>>().Result;

            return View(books);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {

            string url = "booksdata/findbook/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            BookDto selectedBook = response.Content.ReadAsAsync<BookDto>().Result;
            Debug.WriteLine("Book received:");
            Debug.WriteLine(selectedBook.Title);

            return View(selectedBook);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Book/New
        public ActionResult New()
        {
            string url = "authorsdata/listauthors";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<AuthorDto> AuthorOptions = response.Content.ReadAsAsync<IEnumerable<AuthorDto>>().Result;

            return View(AuthorOptions);
        }

        // POST: Book/Create
        [HttpPost]
        public ActionResult Create(Book book)
        {
            string url = "booksdata/addbook";
            string jsonpayload = jss.Serialize(book);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            } else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "booksdata/findbook/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BookDto selectedBook = response.Content.ReadAsAsync<BookDto>().Result;

            return View(selectedBook);
        }

        // POST: Book/Update/5
        [HttpPost]
        public ActionResult Update(int id, Book book)
        {
            string url = "booksdata/updatebook/" + id;
            string jsonpayload = jss.Serialize(book);
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

        // GET: Book/Delete/5
        public ActionResult ConfirmDelete(int id)
        {
            string url = "booksdata/findbook/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BookDto selectedBook = response.Content.ReadAsAsync<BookDto>().Result;

            return View(selectedBook);
        }

        // POST: Book/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "booksdata/deletebook/"+id;
            HttpContent content = new StringContent("");
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
    }
}
