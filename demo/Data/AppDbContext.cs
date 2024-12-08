using Microsoft.EntityFrameworkCore;
using demo.Models;

namespace demo.Data
{
    public class AppDbContext : DbContext
    {
        private readonly string DbPath;

        public DbSet<User> Users { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<UserVote> UserVotes { get; set; }

        public AppDbContext()
        {
            DbPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "UserData.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
            optionsBuilder.LogTo(Console.WriteLine);

            // Подавление предупреждения о незавершённых миграциях
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserVote>()
                .HasKey(uv => uv.Id);

            modelBuilder.Entity<UserVote>()
                .HasIndex(uv => new { uv.UserId, uv.SurveyId })
                .IsUnique();
        }

        public void InitializeDatabase()
        {
            try
            {
                // Применяем миграции
                Console.WriteLine("Applying migrations...");
                Database.Migrate();
                Console.WriteLine("Database initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during database initialization: {ex.Message}");
                throw;
            }
        }
    }
}
