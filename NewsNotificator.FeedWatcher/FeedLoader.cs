using System.Xml.Linq;
using NewsNotificator.Core.Entities;

namespace NewsNotificator.FeedWatcher;

public interface IFeedLoader
{
  Task<XDocument> LoadFeedAsync(Feed feed);
}

public class FeedLoader(HttpClient httpClient) : IFeedLoader
{
  public async Task<XDocument> LoadFeedAsync(Feed feed)
  {
    using var response = await httpClient.GetAsync(feed.Uri);
    response.EnsureSuccessStatusCode();
    var bodyStream = await response.Content.ReadAsStreamAsync();
    return XDocument.Load(bodyStream);
  }
}