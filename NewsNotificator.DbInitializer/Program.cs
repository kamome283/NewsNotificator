using Microsoft.EntityFrameworkCore;
using NewsNotificator.Core;
using NewsNotificator.Core.Repository;
using NewsNotificator.Core.Seeding;

var builder = Host.CreateApplicationBuilder(args);
builder.AddCoreServices();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

file class Worker(ILogger<Worker> logger, IHostApplicationLifetime lifetime, IServiceProvider serviceProvider)
  : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    try
    {
      using var scope = serviceProvider.CreateScope();
      var db = scope.ServiceProvider.GetRequiredService<Db>();
      await db.Database.MigrateAsync(stoppingToken);

      var seeders = scope.ServiceProvider.GetRequiredService<IEnumerable<ISeeder>>();
      foreach (var seeder in seeders)
      {
        await seeder.SeedEntitiesAsync(stoppingToken);
      }

      await db.SaveChangesAsync(stoppingToken);
      lifetime.StopApplication();
    }
    catch (Exception e)
    {
      logger.LogCritical(e, "Failed to migrate database");
      Environment.ExitCode = 1;
      throw;
    }
  }
}
