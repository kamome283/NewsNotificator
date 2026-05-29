// ReSharper disable UnusedVariable

using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var db = builder
  .AddSqlite("db");

var core = builder
  .AddProject<NewsNotificator_Core>("core")
  .WithReference(db)
  .WaitFor(db);

builder.Build().Run();
