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

using BookReviews.Pages;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BookReviews.Controllers
{
    public class BooksController : ControllerBase
    {
        [HttpGet("{bookKey}")]
        public IActionResult GetBookDetails(string bookKey)
        {
            bookKey = WebUtility.UrlDecode(bookKey);
            var docsJson = HttpContext.Session.GetString("Docs");
            var docs = JsonSerializer.Deserialize<List<Doc>>(docsJson);
            var book = docs.FirstOrDefault(doc => doc.key == bookKey);

            if (book != null)
            {
                return new JsonResult(book);
            }
            else
            {
                Console.WriteLine("hlelo");
                return NotFound();
            }
        }
    }
}
