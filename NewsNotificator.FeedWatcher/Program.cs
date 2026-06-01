using NewsNotificator.Core;

var builder = Host.CreateApplicationBuilder(args);
builder.AddCoreServices();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

public class Worker(ILogger<Worker> logger) : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      if (logger.IsEnabled(LogLevel.Information))
      {
        logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
      }

      await Task.Delay(1000, stoppingToken);
    }
  }
}
