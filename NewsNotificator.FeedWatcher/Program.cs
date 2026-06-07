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

  protected override async Task ExecuteAsync(CancellationToken ct)
  {
    try
    {
      while (!ct.IsCancellationRequested)
      {
        await Loop(ct);
        await Task.Delay(PollingInterval, ct);
      }
    }
    catch (Exception)
    {
      Environment.ExitCode = 1;
      throw;
    }
  }

  private async Task Loop(CancellationToken ct)
  {
    using var scope = serviceProvider.CreateScope();
    var writableRssDomain = scope.ServiceProvider.GetRequiredService<WritableRssDomain>();
    var writableEntryDomain = scope.ServiceProvider.GetRequiredService<WritableEntryDomain>();

    var feeds = writableRssDomain.Feeds;
    foreach (var feed in feeds)
    {
      var document = await feedLoader.LoadFeedAsync(feed);
      var entries = feedParser.ParseXmlAsync(document, feed);
      foreach (var entry in entries)
      {
        if (await writableEntryDomain.Entries.FindAsync([entry.Guid], ct) is not null) continue;
        if (logger.IsEnabled(LogLevel.Information))
          logger.LogInformation("Adding {EntryTitle} to {FeedTitle}", entry.Title, feed.Title);
        await writableEntryDomain.Entries.AddAsync(entry, ct);
      }
      await writableEntryDomain.SaveChangesAsync(ct);
    }
  }
}
