using Microsoft.AspNetCore.Identity;
using MvcLab1.Models;

namespace MvcLab1.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.EnsureCreatedAsync();

            if (context.Users.Any()) return;

            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // Создаем роли
                string[] roles = { "User", "Admin" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Создаем тестового пользователя
                var user = new IdentityUser
                {
                    UserName = "testuser",
                    Email = "test@example.com"
                };

                await userManager.CreateAsync(user, "Test123!");
                await userManager.AddToRoleAsync(user, "User");

                // Создаем админа
                var admin = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@example.com"
                };

                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");

                // Создаем проекты
                var projects = new[]
                {
                    new Project { Name = "Личные дела", Description = "Личные задачи", Color = "FF5733", OwnerId = user.Id, CreatedAt = DateTime.UtcNow },
                    new Project { Name = "Работа", Description = "Рабочие задачи", Color = "33FF57", OwnerId = user.Id, CreatedAt = DateTime.UtcNow },
                    new Project { Name = "Учеба", Description = "Учебные задачи", Color = "3357FF", OwnerId = user.Id, CreatedAt = DateTime.UtcNow }
                };
                await context.Projects.AddRangeAsync(projects);
                await context.SaveChangesAsync();

                // Создаем теги
                var tags = new[]
                {
                    new Tag { Name = "Важное", OwnerId = user.Id },
                    new Tag { Name = "Срочное", OwnerId = user.Id },
                    new Tag { Name = "Идея", OwnerId = user.Id }
                };
                await context.Tags.AddRangeAsync(tags);
                await context.SaveChangesAsync();

                // Создаем задачи (используем новые имена)
                var now = DateTime.UtcNow;
                var tasks = new[]
                {
                    new DiaryTask
                    {
                        Title = "Купить продукты",
                        Description = "Молоко, хлеб, яйца",
                        Status = DiaryTaskStatus.Новая,
                        Priority = DiaryTaskPriority.Средний,
                        UserId = user.Id,
                        ProjectId = projects[0].Id,
                        CreatedAt = now,
                        Deadline = now.AddDays(1)
                    },
                    new DiaryTask
                    {
                        Title = "Сдать отчет",
                        Description = "Подготовить квартальный отчет",
                        Status = DiaryTaskStatus.ВРаботе,
                        Priority = DiaryTaskPriority.Высокий,
                        UserId = user.Id,
                        ProjectId = projects[1].Id,
                        CreatedAt = now,
                        Deadline = now.AddHours(5)
                    },
                    new DiaryTask
                    {
                        Title = "Прочитать книгу",
                        Description = "Глава 3 'Изучаем ASP.NET'",
                        Status = DiaryTaskStatus.Новая,
                        Priority = DiaryTaskPriority.Низкий,
                        UserId = user.Id,
                        ProjectId = projects[2].Id,
                        CreatedAt = now,
                        Deadline = null
                    },
                    new DiaryTask
                    {
                        Title = "Позвонить маме",
                        Description = "",
                        Status = DiaryTaskStatus.Новая,
                        Priority = DiaryTaskPriority.Средний,
                        UserId = user.Id,
                        ProjectId = null,
                        CreatedAt = now,
                        Deadline = now.AddDays(2)
                    }
                };
                await context.Tasks.AddRangeAsync(tasks);
                await context.SaveChangesAsync();

                // Связываем задачи с тегами
                var taskTags = new[]
                {
                    new TaskTag { TaskId = tasks[0].Id, TagId = tags[1].Id },
                    new TaskTag { TaskId = tasks[1].Id, TagId = tags[0].Id },
                    new TaskTag { TaskId = tasks[1].Id, TagId = tags[1].Id },
                    new TaskTag { TaskId = tasks[2].Id, TagId = tags[2].Id }
                };
                await context.TaskTags.AddRangeAsync(taskTags);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();

                Console.WriteLine("Seed data created successfully!");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error creating seed data: {ex.Message}");
                throw;
            }
        }
    }
}