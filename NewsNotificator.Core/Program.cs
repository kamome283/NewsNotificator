using NewsNotificator.Core;
using NewsNotificator.Core.Seeding;

var builder = Host.CreateApplicationBuilder(args);

var conn = Environment.GetEnvironmentVariable("ConnectionStrings__db");
if (conn is null) throw new NullReferenceException("ConnectionStrings__db");
builder.Services.AddSqlite<Db>(conn);
builder.Services.AddScoped<ISeeder, FeedSeeder>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

file class Worker(ILogger<Worker> logger, IHostApplicationLifetime lifetime, IServiceProvider serviceProvider) : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    lifetime.StopApplication();
    // try
    // {
    //   using var scope = serviceProvider.CreateScope();
    //   var db = scope.ServiceProvider.GetRequiredService<Db>();
    //   await db.Database.MigrateAsync(stoppingToken);
    //
    //   var seeders = scope.ServiceProvider.GetRequiredService<IEnumerable<ISeeder>>();
    //   foreach (var seeder in seeders)
    //   {
    //     await seeder.SeedEntitiesAsync(stoppingToken);
    //   }
    //   await db.SaveChangesAsync(stoppingToken);
    //   lifetime.StopApplication();
    // }
    // catch (Exception e)
    // {
    //   logger.LogCritical(e, "Failed to migrate database");
    //   Environment.ExitCode = 1;
    //   throw;
    // }
  }
}
