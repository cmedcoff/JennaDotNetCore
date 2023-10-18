using CarDomainLib;
using Microsoft.EntityFrameworkCore;

    public class CarDbContext : DbContext
    {
        public CarDbContext (DbContextOptions<CarDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Car { get; set; } = default!;
    }
