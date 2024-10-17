using System.Collections.Generic;
using Lab1_1.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Lab1_1.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<N018Dictionary> Dictionaries { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=dictionarydb;Username=postgres;Password=123456");
        }
    }
}

