using Microsoft.Extensions.Logging;
using NewsNotificator.Core.Entities;
using NewsNotificator.Core.Repository;

namespace NewsNotificator.Core.Seeding;

public class FeedSeeder(Db db, ILogger<ISeeder<Feed>> logger) : ISeeder<Feed>
{
  public Db Db => db;
  public ILogger<ISeeder<Feed>> Logger => logger;

  public Feed[] Entities =>
  [
    new() { Title = "NHK ONE ニュース", Uri = new Uri("http://www.nhk.or.jp/rss/news/cat0.xml") },
  ];
}
