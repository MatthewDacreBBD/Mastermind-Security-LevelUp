using MastermindService.Models;
using Microsoft.EntityFrameworkCore;

namespace MastermindService.Data
{
    public class MastermindDBContext : DbContext
    {
        public MastermindDBContext(DbContextOptions<MastermindDBContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Game> Game { get; set; }
    }
}