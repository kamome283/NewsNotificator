using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsNotificator.Core.Repository;

namespace NewsNotificator.Core.Seeding;

public interface ISeeder
{
  Task SeedEntitiesAsync(CancellationToken stoppingToken);
}

public interface ISeeder<out T> : ISeeder where T : class
{
  Db Db { get; }
  ILogger<ISeeder<T>> Logger { get; }

  T[] Entities { get; }

  async Task ISeeder.SeedEntitiesAsync(CancellationToken stoppingToken)
  {
    if (Logger.IsEnabled(LogLevel.Information))
      Logger.LogInformation("Seeding entities of type {Type}.", typeof(T).Name);

    var dbSet = Db.Set<T>();
    if (!await dbSet.AnyAsync(stoppingToken))
      await dbSet.AddRangeAsync(Entities, stoppingToken);
  }
}
