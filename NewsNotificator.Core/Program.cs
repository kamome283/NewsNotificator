using Microsoft.EntityFrameworkCore;
using NewsNotificator.Core;

var builder = Host.CreateApplicationBuilder(args);

var conn = Environment.GetEnvironmentVariable("ConnectionStrings__sqlite")!;
builder.Services.AddSqlite<Db>(conn);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

file class Worker(ILogger<Worker> logger, IHostApplicationLifetime lifetime, IServiceProvider serviceProvider) : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    try
    {
      using var scope = serviceProvider.CreateScope();
      var db = scope.ServiceProvider.GetRequiredService<Db>();
      await db.Database.MigrateAsync(stoppingToken);
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
