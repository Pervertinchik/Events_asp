using Events_asp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Events_asp.Database
{
    public class AppDBContext: DbContext
    {
        public DbSet<EventDate> event_dates {  get; set; } = null!;
        public DbSet<EventTime> event_times { get; set; } = null!;
        public DbSet<User> users { get; set; } = null!;

        public AppDBContext()
        {
            
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = WebApplication.CreateBuilder();
            var connectionString = builder.Configuration.GetConnectionString("Default");
            optionsBuilder.UseSqlServer(connectionString);
        }

    }
}
