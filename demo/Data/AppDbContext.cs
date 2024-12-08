using Microsoft.EntityFrameworkCore;
using demo.Models;
using System.IO;

namespace demo.Data
{
    public class AppDbContext : DbContext
    {
        private readonly string DbPath;

        public DbSet<User> Users { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<UserVote> UserVotes { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; } // Таблица отзывов

        public AppDbContext()
        {
            DbPath = Path.Combine(FileSystem.AppDataDirectory, "UserData.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
            optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserVote>().HasKey(uv => uv.Id);
            modelBuilder.Entity<UserVote>().HasIndex(uv => new { uv.UserId, uv.SurveyId }).IsUnique();

            // Конфигурация для таблицы отзывов 222
            modelBuilder.Entity<Feedback>().HasKey(f => f.Id);
        }

        /// <summary>
        /// Проверка наличия миграций и их применение.
        /// </summary>
        public void InitializeDatabase()
        {
            try
            {
                Console.WriteLine("Checking for pending migrations...");
                var pendingMigrations = Database.GetPendingMigrations();

                if (pendingMigrations.Any())
                {
                    Console.WriteLine("Applying pending migrations...");
                    Database.Migrate(); // Применение миграций без удаления данных
                    Console.WriteLine("Migrations applied successfully.");
                }
                else
                {
                    Console.WriteLine("No pending migrations. Database is up to date.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during database initialization: {ex.Message}");
                throw;
            }
        }
    }
}
