using Core;
using Xunit;

namespace Tests.Templates
{
    [Collection("DatabaseConnected")]
    public class DatabaseTestCase
    {
        protected readonly AppDbContext DbContext;

        protected DatabaseTestCase()
        {
            DbContext = Setup.GetDbContext();
        }
    }
}
