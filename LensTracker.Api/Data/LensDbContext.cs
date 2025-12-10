using LensTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LensTracker.Api.Data
{
    public class LensDbContext : DbContext
    {
        public LensDbContext(DbContextOptions<LensDbContext> options) : base(options)
        {
        }

        public DbSet<LensSession> LensSessions { get; set; }
    }
}
