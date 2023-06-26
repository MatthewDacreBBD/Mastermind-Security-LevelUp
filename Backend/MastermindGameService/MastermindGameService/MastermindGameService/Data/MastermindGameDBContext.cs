using MastermindGameService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MastermindGameService.Data
{
    public class MastermindGameDBContext : DbContext
    {
        public MastermindGameDBContext(DbContextOptions<MastermindGameDBContext> options) : base(options)
        {
        }

        public DbSet<Game> Game { get; set; }
    }
}