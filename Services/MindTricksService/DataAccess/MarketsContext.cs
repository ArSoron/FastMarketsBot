using FastMarkets.MindTricksService.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace FastMarkets.MindTricksService.DataAccess
{
    public class MarketsContext : DbContext
    {
        public MarketsContext(string connectionString)
            : base(new DbContextOptionsBuilder<MarketsContext>().UseMySQL(connectionString).Options)
        { }

        public DbSet<Market> Markets { get; set; }
    }
}
