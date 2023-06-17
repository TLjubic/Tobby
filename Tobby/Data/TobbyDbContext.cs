using Microsoft.EntityFrameworkCore;
using Tobby.Models;

namespace Tobby.Data
{
    public class TobbyDbContext : DbContext
    {
        public TobbyDbContext(DbContextOptions<TobbyDbContext> options) : base(options) { }

        public DbSet<Element> Element { get; set; }
    }
}
