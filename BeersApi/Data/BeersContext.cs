using BeersApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BeersApi.Data
{
    public class BeersContext : DbContext
    {
        public DbSet<Beer> Beers { get; set; }
        public DbSet<BeerType> BeerTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<User> Users { get; set; }

        public BeersContext(DbContextOptions<BeersContext> options) : base(options)
        { }
    }
}