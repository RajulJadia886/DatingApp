using Microsoft.EntityFrameworkCore;
using DatingAppAPI.Entities;

namespace DatingAppAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }
    }
}