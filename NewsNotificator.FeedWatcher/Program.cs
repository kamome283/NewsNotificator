using Microsoft.EntityFrameworkCore;
using NewsNotificator.Core;
using NewsNotificator.Core.Domains;

var builder = Host.CreateApplicationBuilder(args);
builder.AddCoreServices();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

file class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider) : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    try
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        using var scope = serviceProvider.CreateScope();
        var writableRssDomain = scope.ServiceProvider.GetRequiredService<WritableRssDomain>();
        if (logger.IsEnabled(LogLevel.Information))
        {
          logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
          var feeds = writableRssDomain.Feeds;
          foreach (var feed in feeds)
          {
            logger.LogInformation("Feed: {Id}, {Title}, {URL}", feed.Id, feed.Title, feed.Uri);
          }
        }

        await Task.Delay(1000, stoppingToken);
      }
    }
    catch (Exception)
    {
      Environment.ExitCode = 1;
      throw;
    }
  }
}
