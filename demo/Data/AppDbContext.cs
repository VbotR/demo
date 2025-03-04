using Microsoft.EntityFrameworkCore;
using demo.Models;
using System;
using System.IO;
using System.Linq;

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

            try
            {
                // Удаляем старую базу данных перед созданием новой
                if (File.Exists(DbPath))
                {
                    File.Delete(DbPath);
                    Console.WriteLine("Старая база данных удалена.");
                }

                // Создаем новую базу данных
                Database.EnsureCreated();
                Console.WriteLine("Новая база данных создана.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении/создании базы данных: {ex.Message}");
            }
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

            // Конфигурация для таблицы отзывов
            modelBuilder.Entity<Feedback>().HasKey(f => f.Id);
        }

        /// <summary>
        /// Проверка наличия миграций и их применение.
        /// </summary>
        public void InitializeDatabase()
        {
            try
            {
                Console.WriteLine("Проверка наличия миграций...");
                var pendingMigrations = Database.GetPendingMigrations();

                if (pendingMigrations.Any())
                {
                    Console.WriteLine("Применение миграций...");
                    Database.Migrate(); // Применение миграций без удаления данных
                    Console.WriteLine("Миграции успешно применены.");
                }
                else
                {
                    Console.WriteLine("Миграции не требуются. База данных актуальна.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при инициализации базы данных: {ex.Message}");
                throw;
            }
        }
    }
}
