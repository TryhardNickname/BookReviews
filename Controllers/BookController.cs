﻿//using BookReviews.Pages;
//using Microsoft.AspNetCore.Mvc;
//using System.Text.Json;

using BookReviews.Data;
using BookReviews.Models;
using BookReviews.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace BookReviews.Controllers
{
    public class BooksController : ControllerBase
    {
        // Two classes to use when decoding the input sent from javascript.
        public class BookUpdateModel
        {
            public string Key { get; set; }
            public int Score { get; set; }
            public string Review { get; set; }
        }

        public class BookKeyModel
        {
            public string Key { get; set; }
        }


        private readonly BookContext _context;

        public BooksController(BookContext context)
        {
            _context = context;
        }

        // Fetch info from the session and return the matching book by key.
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

        // Fetch the info from session and save the doc aka book to library.
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

        // Fetch info about a book from the database by key.
        [HttpGet]
        [Route("Books/DetailsFromDb/{id?}")]
        public IActionResult GetBookDetailsFromDb(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                id = WebUtility.UrlDecode(id);
                var book = _context.Books.FirstOrDefault(b => b.Key == id);
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

        //Update a book in DB, the only fields that are changeable are score and review.
        [HttpPost]
        [Route("Books/UpdateBook")]
        public async Task<IActionResult> UpdateBook([FromBody] BookUpdateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Key))
            {
                return BadRequest();
            }

            var book = _context.Books.FirstOrDefault(b => b.Key == model.Key);
            if (book != null)
            {
                book.Score = model.Score;
                book.Review = model.Review;

                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }

        //Delete book from the DB by key.
        [HttpPost]
        [Route("Books/DeleteBook")]
        public async Task<IActionResult> DeleteBook([FromBody] BookKeyModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Key))
            {
                return BadRequest();
            }

            var book = _context.Books.FirstOrDefault(b => b.Key == model.Key);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }

        //Create a Book from a Doc and save to DB.
        private async Task SaveBookToLibrary(Doc book)
        {
            // Convert Doc to Book
            var dbBook = new Book
            {
                Key = book.key,
                Title = book.title,
                AuthorName = string.Join(",",book.author_name),
                Subject = string.Join(",", book.subject),
                Isbn = string.Join(",", book.isbn),
                FirstPublishYear = book.first_publish_year,
                Publisher = string.Join(",", book.publisher),
                ImageUrl = book.imageUrl,
            };

            using (var context = new BookContext())
            {
                context.Books.Add(dbBook);
                await context.SaveChangesAsync();
            }
        }
    }
}
