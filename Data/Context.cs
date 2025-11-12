using Microsoft.EntityFrameworkCore;
using urlShortenerApi.Models;

namespace urlShortenerApi.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Url> Urls { get; set; }
    }
}
