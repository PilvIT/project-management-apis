using Core;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public static class Setup
    {
        private static readonly DbContextOptions<AppDbContext> DbContextOptions = InitDatabase();

        public static AppDbContext GetDbContext() => new AppDbContext(DbContextOptions);

        private static DbContextOptions<AppDbContext> InitDatabase()
        {
            const string connectionString = "Database=test;Host=127.0.0.1;Port=5432;Username=postgres;Password=Ieg5DvXNel6r5ZE8";
            var dbContextBuilder = new DbContextOptionsBuilder<AppDbContext>();
            dbContextBuilder.UseNpgsql(connectionString);
            
            var db = new AppDbContext(dbContextBuilder.Options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            
            return dbContextBuilder.Options;
        }
    }
}
