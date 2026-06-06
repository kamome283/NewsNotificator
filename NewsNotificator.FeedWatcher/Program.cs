using NewsNotificator.Core;
using NewsNotificator.Core.Domains;

var builder = Host.CreateApplicationBuilder(args);
builder.AddCoreServices();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

file class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration config) : BackgroundService
{
  private TimeSpan PollingInterval =>
    config.GetValue<TimeSpan?>("PollingInterval")
    ?? throw new NullReferenceException("Polling interval not specified");

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

        await Task.Delay(PollingInterval, stoppingToken);
      }
    }
    catch (Exception)
    {
      Environment.ExitCode = 1;
      throw;
    }
  }
}
