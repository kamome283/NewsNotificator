using Microsoft.EntityFrameworkCore;
using NewsNotificator.Core.Entities;

namespace NewsNotificator.Core;

public class Db(DbContextOptions<Db> options): DbContext(options)
{
  internal DbSet<Feed> Feeds { get; set; }
}
