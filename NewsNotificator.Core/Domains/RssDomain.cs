using Microsoft.EntityFrameworkCore;
using NewsNotificator.Core.Entities;
using NewsNotificator.Core.Repository;

namespace NewsNotificator.Core.Domains;

public class WritableRssDomain(Db db)
{
  public DbSet<Feed> Feeds => db.Feeds;
  public DbSet<Entry> Entries => db.Entries;

  public int SaveChanges()
  {
    return db.SaveChanges();
  }

  public Task<int> SaveChangesAsync()
  {
    return db.SaveChangesAsync();
  }
}
