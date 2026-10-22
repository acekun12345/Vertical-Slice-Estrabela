namespace Infrastructure.Persistence;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DbSeeder
{
    public static async Task MigrateAndSeedAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Apply any pending migrations automatically
        await context.Database.MigrateAsync();

        // Idempotent Seeding for Students
        if (!await context.Students.AnyAsync())
        {
            context.Students.AddRange(
                new Student
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    DateOfBirth = new DateTime(2000, 1, 15)
                },
                new Student
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    DateOfBirth = new DateTime(2001, 5, 20)
                }
            );
        }

        // Idempotent Seeding for Courses
        if (!await context.Courses.AnyAsync())
        {
            context.Courses.AddRange(
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "Introduction to Programming",
                    Code = "CS101",
                    Credits = 3
                },
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "Data Structures",
                    Code = "CS201",
                    Credits = 4
                },
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "Database Systems",
                    Code = "CS301",
                    Credits = 3
                }
            );
        }

        await context.SaveChangesAsync();
    }
}