using Microsoft.EntityFrameworkCore;
using NewsNotificator.Core.Entities;
using NewsNotificator.Core.Repository;

namespace NewsNotificator.Core.Domains;

public class WritableEntryDomain(Db db)
{
  public DbSet<Entry> Entries => db.Entries;

  public int SaveChanges()
  {
    return db.SaveChanges();
  }

  public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return db.SaveChangesAsync(cancellationToken);
  }
}
