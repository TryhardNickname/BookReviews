using Microsoft.EntityFrameworkCore;
using BookReviews.Models; // Update this with the correct namespace

namespace BookReviews.Data
{
    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; } // Set of LibraryBook

        public BookContext(DbContextOptions<BookContext> options) : base(options)
        { }

        public BookContext() : base() { }
         
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=library.db"); // Name your database file here
        }
    }
}
