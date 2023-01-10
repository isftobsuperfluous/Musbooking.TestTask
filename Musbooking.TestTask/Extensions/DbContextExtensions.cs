using Microsoft.EntityFrameworkCore;
using Musbooking.TestTask.Infrastructure;
using Musbooking.TestTask.Infrastructure.Entities;

namespace Musbooking.TestTask.Extensions;

public static class DbContextExtensions
{
    public static async Task SeedAsync(this ApplicationDbContext dbContext)
    {
        if (!await dbContext.Equipment.AnyAsync())
        {
            var equipment = new List<Equipment>
            {
                new("Digital audio workstation", 10),
                new("Microphone", 20),
                new("Microphone stands", 20),
                new("Headphone", 30),
                new("Studio chairs", 40)
            };

            await dbContext.Equipment.AddRangeAsync(equipment);
            await dbContext.SaveChangesAsync();
        }
    }
}