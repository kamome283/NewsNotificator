using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NewsNotificator.Core.Repository;

// ReSharper disable once UnusedType.Global
public class DesignTimeDbFactory : IDesignTimeDbContextFactory<Db>
{
  public Db CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<Db>()
      .UseSqlite("DataSource=:memory:");
    return new Db(optionsBuilder.Options);
  }
}
