//using BookReviews.Pages;
//using Microsoft.AspNetCore.Mvc;
//using System.Text.Json;

//namespace BookReviews.Controllers
//{
//    public class BookController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}

using BookReviews.Data;
using BookReviews.Models;
using BookReviews.Pages;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BookReviews.Controllers
{
    public class BooksController : ControllerBase
    {
        [HttpGet]
        [Route("Books/{id?}")]
        public IActionResult GetBookDetails(string id)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(HttpContext.Session.GetString("Docs"))) 
            {
                id = WebUtility.UrlDecode(id);
                var docsJson = HttpContext.Session.GetString("Docs");
                var docs = JsonSerializer.Deserialize<List<Doc>>(docsJson);
                var book = docs.FirstOrDefault(doc => doc.key == id);

                if (book != null)
                {
                    return new JsonResult(book);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("Books/{id?}")]
        public async Task<IActionResult> AddBookToLibrary(string id)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(HttpContext.Session.GetString("Docs")))
            {
                id = WebUtility.UrlDecode(id);
                var docsJson = HttpContext.Session.GetString("Docs");
                var docs = JsonSerializer.Deserialize<List<Doc>>(docsJson);
                var book = docs.FirstOrDefault(doc => doc.key == id);
                await SaveBookToLibrary(book);
                if (book != null)
                {
                    return new JsonResult(new { success = true, message = "Book added to the library" });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }

        }
        private async Task SaveBookToLibrary(Doc book)
        {
            // Convert Doc to LibraryBook
            var dbBook = new Book
            {
                Key = book.key,
                Title = book.title,
                AuthorName = string.Join(",",book.author_name),
                Subject = string.Join(",", book.subject),
                Isbn = string.Join(",", book.isbn),
                FirstPublishYear = book.first_publish_year,
                Publisher = string.Join(",", book.publisher),
                ImageUrl = book.imageUrl
                // Assign other properties as needed
            };

            // Save to database using BookContext
            using (var context = new BookContext())
            {
                context.Books.Add(dbBook);
                await context.SaveChangesAsync();
            }
        }
    }
}
