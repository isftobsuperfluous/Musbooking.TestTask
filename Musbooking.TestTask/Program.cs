using Microsoft.EntityFrameworkCore;
using Musbooking.TestTask.Extensions;
using Musbooking.TestTask.Infrastructure;
using Musbooking.TestTask.ServiceAbstractions;
using Musbooking.TestTask.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var configuration = builder.Configuration.AddJsonFile("appsettings.Development.json").Build();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("DevConnection"));
});

builder.Services.AddDbContext<InMemoryDbContext>(options =>
{
    options.UseInMemoryDatabase(databaseName: "BookedEquipment");
});

builder.Services.AddTransient<IEquipmentService, EquipmentService>();

var app = builder.Build();

app.Lifetime.ApplicationStopping.Register(OnShutDown);

app.MapControllers();

await app.Services.MigrateDatabaseAsync();

async void OnShutDown()
{
    await using (var scope = app.Services.CreateAsyncScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var inMemoryDbContext = scope.ServiceProvider.GetRequiredService<InMemoryDbContext>();

        var ids = await inMemoryDbContext.Equipment.Select(x => x.Id).ToArrayAsync();
        var amounts = await inMemoryDbContext.Equipment.Select(x => x.Amount).ToArrayAsync();

        var equipmentUpdate = dbContext.Equipment.Where(x => ids.Contains(x.Id));

        var index = 0;
        await equipmentUpdate.ForEachAsync(x =>
        {
            x.Amount += amounts[index];
            index++;
        });

        await dbContext.SaveChangesAsync();
    }
}

app.Run();