using System.Xml.Linq;
using NewsNotificator.Core.Entities;

namespace NewsNotificator.FeedWatcher;

public interface IFeedParser
{
  IEnumerable<Entry> ParseXmlAsync(XDocument document, Feed feed);
}

public class FeedParser : IFeedParser
{
  public IEnumerable<Entry> ParseXmlAsync(XDocument document, Feed feed)
  {
    return
      from item in document.Descendants("item")
      let title = item.Element("title")!.Value
      let link = new Uri(item.Element("link")!.Value)
      let guid = item.Element("guid")!.Value
      let description = item.Element("description")?.Value
      let pubDate = DateTime.Parse(item.Element("pubDate")!.Value)
      select new Entry
      {
        Title = title,
        Description = description,
        Guid = guid,
        Link = link,
        PubDate = pubDate,
        Feed = feed,
      };
  }
}
