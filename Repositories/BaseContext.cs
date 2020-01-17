using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.EntitiesConfig;

namespace Repositories
{
    public class BaseContext : DbContext
    {
        public BaseContext() : base()
        {
            Database.Migrate();
        }

        public BaseContext(DbContextOptions<BaseContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}