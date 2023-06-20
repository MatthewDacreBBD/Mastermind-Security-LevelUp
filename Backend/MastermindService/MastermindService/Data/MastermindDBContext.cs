using MastermindService.Models;
using Microsoft.EntityFrameworkCore;

namespace MastermindService.Data
{
    public class MastermindDBContext : DbContext
    {
        public MastermindDBContext(DbContextOptions<MastermindDBContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Game> Game { get; set; }
    }
}