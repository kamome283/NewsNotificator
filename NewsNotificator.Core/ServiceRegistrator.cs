using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsNotificator.Core.Repository;
using NewsNotificator.Core.Seeding;

namespace NewsNotificator.Core;

public static class ServiceRegistrator
{
  extension(HostApplicationBuilder builder)
  {
    public void AddCoreServices()
    {
      builder.AddDb();
      builder.AddSeeders();
    }

    private void AddDb()
    {
      var connStr = Environment.GetEnvironmentVariable("ConnectionStrings__db");
      if (connStr is null) throw new NullReferenceException("ConnectionStrings__db");
      builder.Services.AddSqlite<Db>(connStr);
    }

    private void AddSeeders()
    {
      builder.Services.AddScoped<ISeeder, FeedSeeder>();
    }
  }
}
