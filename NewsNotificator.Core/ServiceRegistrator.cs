using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsNotificator.Core.Domains;
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
      builder.AddDomains();
      builder.AddSeeders();
    }

    private void AddDb()
    {
      var connStr = Environment.GetEnvironmentVariable("ConnectionStrings__db");
      if (connStr is null) throw new NullReferenceException("ConnectionStrings__db");
      builder.Services.AddSqlite<Db>(connStr);
    }

    private void AddDomains()
    {
      builder.Services.AddTransient<WritableRssDomain>();
    }

    private void AddSeeders()
    {
      builder.Services.AddScoped<ISeeder, FeedSeeder>();
    }
  }
}
