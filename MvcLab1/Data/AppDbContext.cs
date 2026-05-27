using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MvcLab1.Models;

namespace MvcLab1.Data
{
    // Добавляем IdentityDbContext для аутентификации
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Существующие DbSet
        public DbSet<Game> Games { get; set; }
        public DbSet<Product> Products { get; set; }

        // НОВЫЕ DbSet для Умного ежедневника
        public DbSet<Project> Projects { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<DiaryTask> Tasks { get; set; }
        public DbSet<TaskTag> TaskTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ВАЖНО: вызываем base для Identity

            // ===== Настройки для Game =====
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Title).IsRequired().HasMaxLength(100);
                entity.Property(g => g.Genre).IsRequired().HasMaxLength(50);
                entity.Property(g => g.Platform).IsRequired().HasMaxLength(50);
                entity.Property(g => g.Developer).IsRequired().HasMaxLength(100);
                entity.Property(g => g.Price).HasColumnType("decimal(18,2)");
                entity.HasIndex(g => g.Genre).HasDatabaseName("IX_Games_Genre");
            });

            // ===== Настройки для Product =====
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
                entity.Property(p => p.Category).HasMaxLength(50);
                entity.HasIndex(p => p.Category).HasDatabaseName("IX_Products_Category");
            });

            // ===== НОВЫЕ настройки для Умного ежедневника =====

            // Уникальность Email и UserName
            modelBuilder.Entity<IdentityUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<IdentityUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            // Уникальность тега в рамках пользователя
            modelBuilder.Entity<Tag>()
                .HasIndex(t => new { t.Name, t.OwnerId })
                .IsUnique();

            // Уникальность проекта в рамках пользователя
            modelBuilder.Entity<Project>()
                .HasIndex(p => new { p.Name, p.OwnerId })
                .IsUnique();

            // Индексы для оптимизации (доп. задание)
            modelBuilder.Entity<DiaryTask>()
                .HasIndex(t => t.Status)
                .HasDatabaseName("IX_Tasks_Status");

            modelBuilder.Entity<DiaryTask>()
                .HasIndex(t => t.Priority)
                .HasDatabaseName("IX_Tasks_Priority");

            modelBuilder.Entity<DiaryTask>()
                .HasIndex(t => new { t.Deadline, t.Status })
                .HasDatabaseName("IX_Tasks_Deadline_Status");

            // Настройка каскадного удаления
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tag>()
                .HasOne(t => t.Owner)
                .WithMany()
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DiaryTask>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DiaryTask>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            // Настройка связи многие-ко-многим
            modelBuilder.Entity<TaskTag>()
                .HasKey(tt => new { tt.TaskId, tt.TagId });

            modelBuilder.Entity<TaskTag>()
                .HasOne(tt => tt.Task)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskTag>()
                .HasOne(tt => tt.Tag)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}