using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NewsNotificator.Core;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<Db>
{
  public Db CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<Db>()
      .UseSqlite("DataSource=:memory:");
    return new Db(optionsBuilder.Options);
  }
}
