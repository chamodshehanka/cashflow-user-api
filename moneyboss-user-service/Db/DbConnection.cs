using Microsoft.EntityFrameworkCore;
using moneyboss_user_service.Models;

namespace moneyboss_user_service.Db
{
    public class DbConnection: DbContext
    {
        public DbConnection(DbContextOptions<DbConnection> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            base.OnConfiguring(optionBuilder);
            optionBuilder.UseSqlServer("Server=tcp:moneyboss.database.windows.net,1433;Initial Catalog=moneyboss;Persist Security Info=False;User ID=kaveesha;Password=Kaviya@98;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        public DbSet<User> Users => Set<User>();
    }
}
