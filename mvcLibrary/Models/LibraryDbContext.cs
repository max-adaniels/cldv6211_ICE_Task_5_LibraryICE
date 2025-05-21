using Microsoft.EntityFrameworkCore;
namespace mvcLibrary.Models  
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Book> Book { get; set; }
        public DbSet<BookType> BookType { get; set; }
        public DbSet<Loan> Loan { get; set; }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }
    }
}
