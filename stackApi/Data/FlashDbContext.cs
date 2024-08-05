using Microsoft.EntityFrameworkCore;
using stackApi.Model.Entities;

namespace stackApi.Data
{
    public class FlashDbContext : DbContext
    {

        public FlashDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Flashcard> Flash { get; set; }

    }
}
