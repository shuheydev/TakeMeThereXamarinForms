using Microsoft.EntityFrameworkCore;
using System;
using TakeMeThereXamarinForms.Models;

namespace TakeMeXamarinForms.Sqlite
{
    public class AppDbContext : DbContext
    {
        private static AppDbContext _dbContext;
        private readonly string _dbPath;

        public DbSet<Place> Places { get; set; }

        private AppDbContext(string dbPath)
        {
            this._dbPath = dbPath;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source = {_dbPath}");
        }

        public static AppDbContext GetInstance(string dbPath)
        {
            return _dbContext = _dbContext ?? new AppDbContext(dbPath);
        }
    }
}
