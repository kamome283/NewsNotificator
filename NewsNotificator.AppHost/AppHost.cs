// ReSharper disable UnusedVariable
var builder = DistributedApplication.CreateBuilder(args);

var db = builder
  .AddSqlite("db");

builder.Build().Run();
