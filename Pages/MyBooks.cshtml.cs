using BookReviews.Data;
using BookReviews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookReviews.Pages
{
    public class MyBooksModel : PageModel
    {
        private readonly BookContext _context;

        public MyBooksModel(BookContext context)
        {
            _context = context;
        }

        public List<Book> Books { get; set; }

        public void OnGet()
        {
            Books = _context.Books.ToList();
            Console.WriteLine(Books);
        }
    }
}
