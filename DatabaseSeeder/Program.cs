// The default connection string is for development purposes! It is same as in defined in Main/docker-compose.yaml

using Core;
using Core.Features.Projects.Models;
using Microsoft.EntityFrameworkCore;

const string defaultConnectionString = "Database=main;Host=127.0.0.1;Port=5432;Username=postgres;Password=Ieg5DvXNel6r5ZE8";
var connectionString = defaultConnectionString;
if (args.Length == 2)
{
    connectionString = args[1];
}

var dbContextBuilder = new DbContextOptionsBuilder<AppDbContext>();
dbContextBuilder.UseNpgsql(connectionString);

var dbContext = new AppDbContext(dbContextBuilder.Options);

// Call the seeding methods below
dbContext.PackageManagers.Add(new PackageManager
{

});


