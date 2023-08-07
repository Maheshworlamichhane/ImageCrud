using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ImageCrud.Models
{
    public class AppDbContext: DbContext
    {
        public DbSet<Image> Images { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
