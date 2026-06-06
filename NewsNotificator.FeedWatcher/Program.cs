using NewsNotificator.Core;
using NewsNotificator.Core.Domains;
using NewsNotificator.Core.Entities;
using NewsNotificator.FeedWatcher;

var builder = Host.CreateApplicationBuilder(args);
builder.AddCoreServices();
builder.Services
  .AddTransient<IFeedLoader, FeedLoader>()
  .AddSingleton<HttpClient>()
  .AddSingleton<IFeedParser, FeedParser>()
  .AddHostedService<Worker>();

var host = builder.Build();
host.Run();

file class Worker(
  ILogger<Worker> logger,
  IServiceProvider serviceProvider,
  IConfiguration config,
  IFeedLoader feedLoader,
  IFeedParser feedParser
) :
  BackgroundService
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
        var feeds = writableRssDomain.Feeds;
        foreach (var feed in feeds)
        {
          await PollFeedAsync(feed);
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

  private async Task PollFeedAsync(Feed feed)
  {
    var document = await feedLoader.LoadFeedAsync(feed);
    var entries = feedParser.ParseXmlAsync(document, feed);
    if (logger.IsEnabled(LogLevel.Information))
    {
      foreach (var entry in entries)
      {
        logger.LogInformation("Entry {Title}: {URL}", entry.Title, entry.Link);
      }
    }
  }
}
