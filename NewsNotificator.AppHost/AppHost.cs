// ReSharper disable UnusedVariable

using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var db = builder
  .AddSqlite("db");

var dbInitializer = builder
  .AddProject<NewsNotificator_DbInitializer>("db-initializer")
  .WithReference(db)
  .WaitFor(db);

var feedWatcher = builder
  .AddProject<NewsNotificator_FeedWatcher>("feed-watcher")
  .WithReference(db)
  .WaitForCompletion(dbInitializer);

var web = builder
  .AddProject<NewsNotificator_Web>("web")
  .WithReference(db)
  .WaitForCompletion(dbInitializer);

builder.Build().Run();