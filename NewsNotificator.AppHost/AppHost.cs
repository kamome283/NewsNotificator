// ReSharper disable UnusedVariable

using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var db = builder
  .AddSqlite("db");

var core = builder
  .AddProject<NewsNotificator_Core>("core")
  .WithInitialState(new CustomResourceSnapshot
  {
    ResourceType = nameof(ProjectResource),
    Properties = [],
    IsHidden = true,
  });

var dbInitializer = builder
  .AddProject<NewsNotificator_DbInitializer>("db-initializer");

foreach (var resource in builder.Resources)
{
  if (resource == db.Resource) continue;

  if (resource is IResourceWithEnvironment env)
  {
    builder.CreateResourceBuilder(env).WithReference(db);
  }

  if (resource is IResourceWithWaitSupport wait)
  {
    builder.CreateResourceBuilder(wait).WaitFor(db);
  }
}

builder.Build().Run();
