using FastMarkets.MindTricksService.Abstractions;
using FastMarkets.MindTricksService.DataAccess;
using System.Linq;

namespace FastMarkets.MindTricksService.Data
{
    public class MySqlMarkets
    {
        private readonly MarketsContext _dbContext;

        public MySqlMarkets(MarketsContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<Market> GetMarkets()
        {
            return _dbContext.Markets;
        }
    }
}
